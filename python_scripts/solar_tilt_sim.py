#!/usr/bin/env python

import math


"""
Airship. Simplified as cylinder.
"""
# diameter in m
bDiameter = 1.7

"""
Photovoltaic.
KISS for speed: 
    1. All modules interpreted as one area
    2. Placed in the center on top.
"""
sLong = 0.96                    # Longitudinal length of module area in m^2
sLat = 1.3                      # Lateral length of module area in m^2
ETA_PH = 0.0433                  # Efficiency per module of 4.33%


"""
Irradiances. (21.6.)
"""
OP_DAY_MID_SUMMER = 173                # Day number in the year 21st June 2020

MAX_SOLAR_ELEVATION_50Lat = 62.4           # Solar elevation angle (degrees)
MAX_SOLAR_ELEVATION_0Lat = 90
I_GLOB_MAX = 1000                
       
GEO_HEIGHT_DRESDEN = 112                # Geodestic height in m over normal zero.
GEO_HEIGHT_SANTODOMINGO = 655
GEO_HEIGHT_QUITO = 2850
GEO_HEIGHT_ZACATECAS = 2440

TURBIDITY_DRESDEN = 5.1                 # TURBIDITY_DRESDEN factor
GAMMA_S = 0                     # Azimuth sun
GAMMA = 0                       # Azimuth photovoltaic

    
"""
TEST parameters: Only for testing purposes. 
derived from the on specific example I calculated for Dresden 21st June
    - Solar elevation angle: 62.4 deg
    - Solar azimuth: 0 deg
    - Photvoltaic azimuth: 0 deg
"""   
# Diffuse and direct irradiance based on Gassel 
TEST_S = 1404.9454549               
TEST_I_ex = 1245.0676341           
TEST_Q = 9.3211615
TEST_I_b = 720.4014021
TEST_I_d_1000Wm2 = 224.3607139
TEST_BETA_30 = 30                   # Tilt angle in degrees
TEST_SIN_THETA = 0.9991228          # Sin Thetha / sin h = 1.12841 mit h=62.4 and beta=30
TEST_I_bT = 812.1927102
TEST_A_I = 0.5786012
TEST_I_dT = 234.5683343
TEST_I_T = 1046.76
# Circle segment
TEST_CENTRAL_ANGLE = 87.62847152    # 
TEST_STEP_WIDTH = 0.0148354           # For central_angle=87.62847152 and sLat=1.3 
TEST_RANGE_BEGIN = 46.18576
TEST_RANGE_END = 133.82
TEST_TILT_ANGLE_4711 = 42.89        # For input phi=47.11




"""
=== Cylinder Cross Section Circle Segment ===========================================================
"""
def central_seg_angle(r, b):
    hSeg = r - math.cos(math.radians( (360 * b)/(4 * math.pi * r) ) ) * r
    alpha = 2 * math.degrees(math.acos(1 - (hSeg / r)))
    return alpha

#Not really needed
def step_width(length, phi):
    return length / phi;

# Returns the tilt angle of a module segment at a specific rotation angle
def tilt_angle(phi):
    if phi > 180:
        return False
    return 90 - phi

def calc_scaling_factor(alpha, beta, mode=0):
    """ Scaling factor for the longitudinal strips, based on 'mode' for angle range alpha to beta.

    Keyword arguments:
    alpha -- Lower angle
    beta  -- Higher angle
    mode -- Determines the type of scaling that should be done (default mode=0: Step for each degree)
    """
    return 1 / (beta - alpha)

def calc_ph_area(sLat, sLong):
    """
    Returns the area of a surface in m^2.
    
    Keyword arguments:
    sLat -- Lateral length in m^2
    sLong -- Longitudinal length in m^2
    """
    return sLat * sLong

"""
=== Irradiances based on Gassel ===========================================================
"""  

# Solar constant
def S(n):
    return 1356.5 + 48.5 * math.cos(math.radians(0.0172 * (n - 15)))

 
"""
def h_true(h):
    return h + (1.4705 / (3.0427 + h)) - 0.0158
"""
   

# Extraterrestrial irradiation
def I_ex(s,h):
    return s * math.sin(math.radians(h))


# Faktor Q for mightiness of air mass
def Q(h, geoHeightDresden):    
    a = round( (9.38076 * (math.sin(math.radians(h)) + math.sqrt(0.003 + (math.sin(math.radians(h))*math.sin(math.radians(h)))))), 6 )
    b = round(2.0015 * (1 - geoHeightDresden * 0.0001) , 6)# + 0.91202
    return a / b + 0.91202


# Direct irradiance 
def I_b(iex,q, TURBIDITY_DRESDEN):
    return iex * math.exp(-(TURBIDITY_DRESDEN/q))


# Diffused irradiance based on correlation approximation from 
def I_d(irrGlobal, irrEx):
    return irrGlobal * ( 0.53 - 0.34 * math.atan( 5.5 * (irrGlobal/irrEx) - 3.16)  )


# Sine of theta dependend on solar elevation and tilt angle
def sin_theta(h, beta):
    return math.sin(math.radians(h))*math.cos(math.radians(beta)) \
        + math.cos(math.radians(h)) * math.cos(math.radians(GAMMA_S - GAMMA)) * math.sin(math.radians(beta))
    

# Total direct radiation on tilted surface
def I_bT(I_b, h, beta):
    return I_b * ( sin_theta(h, beta) / math.sin(math.radians(h)) )


# Anisotrope index    
def A_I(I_b, I_ex):
    return I_b / I_ex
    

# Total diffuesed irradiation 
def I_dT(A_I, I_d, beta, h):
    return I_d * (0.5 * (1 - A_I) * (1 + math.cos(math.radians(beta))) + A_I * (sin_theta(h,beta)/math.sin(math.radians(h))))


# Total effective irradiation on tilted surface
def I_T(I_bT, I_dT):
    return I_bT + I_dT


def I_T_For_Tilt(tA, oD, sE, iG, gH, turb):
    """Total irradiation on tilted surface. Assuming the whole panel was tilted.

    Keyword arguments:
    tA -- Tilt angle of the panel.
    oD -- Number of the day of the operation
    sE -- Solar elevation angle (degrees)
    iG -- Maximum global irradiance for that time of year. (This could better be calculated basd on other
          characteristica)
    gH -- Geodestic Height of the location
    turb -- Turbidity factor (based on Gassel)
    """
    # Solar constant
    s = S( oD )
    #print("s: %f" % s )
    # I_ex()
    irrEx = I_ex( s, sE )
    #print("irrEx: %f" % irrEx )
    # Q
    q = Q( sE, gH )
    #print("q: %f" % q )
    # I_b
    irrB = I_b( irrEx, q , turb )
    #print("irrB: %f" % irrB)
    # I_d based on I_glob
    irrD = I_d( iG, irrEx )
    #print("irrD: %f" % irrD )
    # I_bT (here the tilt angle is used)
    irrBT = I_bT(irrB, sE, tA)
    #print("irrBT: %f" % irrBT )
    # A_I 
    Ai = A_I(irrB, irrEx)
    #print("A_i: %f" % Ai )
    # I_dT
    irrDT = I_dT(Ai, irrD, tA, sE)
    #print("irrDT: %f" % irrDT )
    # I_T
    irrTotal = I_T(irrBT, irrDT)
    #print("irrTotal: %f" % irrTotal )
    
    return irrTotal


def P_Ph_Tilt(eta_ph, A_ph, irr_T_Tilt):
    return eta_ph * A_ph * irr_T_Tilt


def power_eff_total_curve(bDia, sLat, sLong, eta_ph, oD, sE, iG, gH, turb):
    """Total effective power assuming the whole panel was tilted

    Keyword arguments:
    bDia -- Body diameter (in meters)
    sLat -- Lateral length of the photovoltaic
    sLong -- Longitudinal length of the photovoltaic
    oD -- Number of the day of the operation
    sE -- Solar elevation angle (degrees)
    gH -- Geodestic Height of the location
    turb -- Turbidity factor (based on Gassel)
    """
    r = bDiameter/2
    cA = central_seg_angle(r, sLat)
    
    # Round the range limits 
    alpha = 90 - (cA / 2)
    beta = 90 + (cA / 2)
    math.ceil(alpha)
    math.floor(beta)
    
    # Build scaling factor on rounded range limits
    SCALING_FACTOR = calc_scaling_factor(alpha, beta)
    
    # List power per Strips
    pPSs = []
    
    # For each strip at the angle..
    for i in range(int(alpha),int(beta)):
        # Array representing photovoltaic strip
        photo_strip = []
        photo_strip.append(i)
        # Calculate tilt angle
        tA = tilt_angle(i)
        photo_strip.append(tA)
        # Solar elevation angle added to tuple
        photo_strip.append(sE)
        # Calculate total irradiance for the strip
        irrTotal = I_T_For_Tilt(tA, oD, sE, iG, gH, turb)
        photo_strip.append(irrTotal)
        # Multiply it with efficiency factor eta
        irrTotalEta = P_Ph_Tilt(eta_ph, calc_ph_area(sLat, sLong), irrTotal)
        photo_strip.append(irrTotalEta)
        # Multiply with scaling factor
        irrTotalEtaScaled = SCALING_FACTOR * irrTotalEta
        photo_strip.append(irrTotalEtaScaled)
        # Add to the list
        pPSs.append(photo_strip)
        #print(sum(strip[4] for strip in pPSs))   

    return pPSs
    
def calc_max_solar_elevation(lat, season=1):
    """
    Calculates the maxium solar elevation angle based on formulas from:
    http://www.geoastro.de/astro/mittag/index.htm
    
    Keyword arguments:
    lat -- Latitude for which max solar elevation angle is calculated
    season -- Value between 0 and 3. Either spring 21.3 (0), summer 21.6 (1), autumn 23.9 (2), winter 22.12 (3). Summer (1) is default.
    """
    if season < 0 or season > 3:
        return False
    if lat < 0 or lat > 90:
        return False
    
    if(season == 0):
        pass
    elif(season == 1):
        return 90 - lat + 23.5
    elif(season == 2):
        pass
    else: # season == 3
        return 90 - lat - 23.5

def print_solar_elevations(season):
    # Summer
    for lat in range(0,91):
        print( " %f°Lat: %f" % (lat, calc_max_solar_elevation(lat, season)) )    


"""
== Test cases =================================================================================
"""
def test__S():
    res = S( OP_DAY_MID_SUMMER ) 
    print("test_S: %.7f" % res)
    if abs(res - TEST_S) < 0.001:
        return True

def test__I_ex():
    #print(I_ex(S(number_of_day), h))
    res = I_ex( TEST_S, MAX_SOLAR_ELEVATION_50Lat )
    print("I_ex: %.7f" % res)
    if abs(res - TEST_I_ex ) < 0.1:
        return True

def test__Q():
    res = Q( MAX_SOLAR_ELEVATION_50Lat, GEO_HEIGHT_DRESDEN )
    print("test_Q: %.7f" % res)
    if abs(res - TEST_Q) < 0.001:
        return True
        
def test__I_b():
    res = I_b( TEST_I_ex, TEST_Q, TURBIDITY_DRESDEN,)
    print("test_I_b %.7f" % res)
    if abs(res - TEST_I_b) < 0.1:
        return True
        
def test__I_d():
    res = I_d( I_GLOB_MAX, TEST_I_ex )
    print("test_I_d: %.7f" % res)
    if abs(res - TEST_I_d_1000Wm2) < 0.1:
        return True

def test__sin_theta():
    res = sin_theta( MAX_SOLAR_ELEVATION_50Lat, TEST_BETA_30 )
    print("test_sin_theta: %.7f" % res)
    if abs(res - TEST_SIN_THETA) < 0.001:
        return True       
        
def test__I_bT():
    res = I_bT( TEST_I_b, MAX_SOLAR_ELEVATION_50Lat, TEST_BETA_30 )
    print("test_I_bT: %.7f" % res)
    if abs(res - TEST_I_bT) < 0.01:
        return True        
 
def test__A_I():
    res = A_I( TEST_I_b, TEST_I_ex )
    print("test_A_I: %.7f" % res)
    if abs(res - TEST_A_I) < 0.001:
        return True 
 
def test__I_dT():
    res = I_dT( TEST_A_I, TEST_I_d_1000Wm2 , TEST_BETA_30, MAX_SOLAR_ELEVATION_50Lat )
    print("test_I_dT: %.7f" % res)
    if abs(res - TEST_I_dT) < 0.1:
        return True
  
def test__I_T():
    res = I_T( TEST_I_bT, TEST_I_dT )
    print("test_I_T: %.7f" % res)
    if abs(res - TEST_I_T) < 0.01:
        return True

def test__central_seg_angle():
    res = central_seg_angle( bDiameter/2, sLat )
    print("central_seg_angle: %.7f" % res)
    if abs(res - TEST_CENTRAL_ANGLE) < 0.001:
        return True        

def test__step_width():
    res = step_width( sLat, TEST_CENTRAL_ANGLE )
    print("step_width: %.7f" % res)
    if abs(res - TEST_STEP_WIDTH) < 0.0001:
        return True

def test__tilt_angle():
    res = tilt_angle( 47.11 )
    print("tilt_angle: %.7f" % res)
    if abs(res - TEST_TILT_ANGLE_4711) < 0.001:
        return True

def test__I_T_For_Tilt():
    res = I_T_For_Tilt( tilt_angle(133.2), OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_50Lat, I_GLOB_MAX, GEO_HEIGHT_DRESDEN, TURBIDITY_DRESDEN )
    print("I_T_For_Tilt: %.7f" % res)
    #if abs(res - TEST_TILT_ANGLE_4711) < 0.001:
    #    return True
    return True

def test__power_eff_total_curve():
    res = power_eff_total_curve(bDiameter, sLat, sLong, ETA_PH, OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_50Lat, I_GLOB_MAX, GEO_HEIGHT_DRESDEN, TURBIDITY_DRESDEN) 
    print(sum(strip[4] for strip in res))   
    #if abs(res - TEST_TILT_ANGLE_4711) < 0.001:
    #    return True
    return True

def tests_failed(i):
    print("Testcase %d failed!" % i)



def test__all():
    """
    On-the fly during development just to facilitate debugging.
    """
    i = 0
    if not test__S():           # 0
        return tests_failed(i)
    i=i+1
    if not test__I_ex():        # 1
        return tests_failed(i)
    i=i+1
    if not test__Q():           # 2
        return tests_failed(i)
    i=i+1
    if not test__I_b():         # 3
        return tests_failed(i)
    i=i+1
    if not test__I_d():         # 4
        return tests_failed(i)
    i=i+1
    if not test__sin_theta():   # 5
        return tests_failed(i)
    i=i+1
    if not test__I_bT():        # 6
        return tests_failed(i)
    i=i+1
    if not test__A_I():         # 7
        return tests_failed(i)
    i=i+1
    if not test__I_dT():        # 8
        return tests_failed(i)
    i=i+1
    if not test__I_T():         # 9
        return tests_failed(i)
    i=i+1
    if not test__central_seg_angle():   # 10
        return tests_failed(i)
    i=i+1
    if not test__step_width():  # 11
        return tests_failed(i)
    i=i+1
    if not test__tilt_angle():  # 12
        return tests_failed(i)
    i=i+1
    
    if not test__I_T_For_Tilt():   # 13
        return tests_failed(i)
    i=i+1
    
    if not test__power_eff_total_curve():     # 14
        return tests_failed(i)
    i=i+1
    
    print("TESTS SUCCESFUL!")


def print_tuples(case):
    """
    Prints the data of all photovoltaic segments/strips. In LaTeX table format.
    """
    print("{$\\theta$} & {$\\beta$} &  {$h$} & {Irr_{T}} & {P_{Ph,tilt}} & {P_{Ph,strip}}")
    for strip in case:
        print("%f & %f & %f & %f & %f & %f\\\\" % (strip[0], strip[1], strip[2], strip[3], strip[4], strip[5]) )
        

def sum_power_strips(data, index):
    """
    Sums up the power of the individual photovoltaic strips.
    """
    return sum(strip[index] for strip in data)
    

def sim():
    """
    Run the simultion for the created model.
    """
    print("Calculation for 21st June under different conditions..")
    
    ####
    print("\n1. Maximum power output for straight photovoltaic area under perfect tilt (perpendicular):")
    irrTiltDresden = I_T_For_Tilt(tilt_angle(MAX_SOLAR_ELEVATION_50Lat), OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_50Lat, I_GLOB_MAX, GEO_HEIGHT_DRESDEN, TURBIDITY_DRESDEN)
    print("\n\t[+] Dresden, Germany, geodetic 112m height, solar elvation 62,4° ==> %f" % P_Ph_Tilt(ETA_PH, calc_ph_area(sLat, sLong), irrTiltDresden))
    
    irrTiltEquDresden90 = I_T_For_Tilt(tilt_angle(MAX_SOLAR_ELEVATION_0Lat), OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_0Lat, I_GLOB_MAX, GEO_HEIGHT_DRESDEN, TURBIDITY_DRESDEN)
    print("\t[+] At equator, geodetic height of Dresden, solar elevation 90°  ==> %f" % P_Ph_Tilt(ETA_PH, calc_ph_area(sLat, sLong), irrTiltEquDresden90))
    
    irrTiltEquDresden624 = I_T_For_Tilt(tilt_angle(MAX_SOLAR_ELEVATION_50Lat), OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_0Lat, I_GLOB_MAX, GEO_HEIGHT_DRESDEN, TURBIDITY_DRESDEN)
    print("\t[+] At equator, geodetic height of Dresden, solar elevation 62,4°  ==> %f" % P_Ph_Tilt(ETA_PH, calc_ph_area(sLat, sLong), irrTiltEquDresden624))
    
    irrTiltSantoDomingo = I_T_For_Tilt(tilt_angle(MAX_SOLAR_ELEVATION_0Lat), OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_0Lat, I_GLOB_MAX, GEO_HEIGHT_SANTODOMINGO, TURBIDITY_DRESDEN)
    print("\t[+] Ecuador, Santo Domingo, ~0°Lat, 655m geodetic height, solar elevation 90° ==> %f" % P_Ph_Tilt(ETA_PH, calc_ph_area(sLat, sLong), irrTiltSantoDomingo))
    
    irrTiltQuito = I_T_For_Tilt(tilt_angle(MAX_SOLAR_ELEVATION_0Lat), OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_0Lat, I_GLOB_MAX, GEO_HEIGHT_QUITO, TURBIDITY_DRESDEN)
    print("\t[+] Ecuador, Quito, ~0°Lat, 2850m geodetic height, solar elevation 90° ==> %f" % P_Ph_Tilt(ETA_PH, calc_ph_area(sLat, sLong), irrTiltQuito))
    
    irrTiltZacatecas = I_T_For_Tilt(tilt_angle(MAX_SOLAR_ELEVATION_0Lat), OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_0Lat, I_GLOB_MAX, GEO_HEIGHT_ZACATECAS, TURBIDITY_DRESDEN)
    print("\t[+] Mexiko, Zacatecas, 23°Lat (norther tropic), 2440m geodetic height, solar elevation 90° ==> %f" % P_Ph_Tilt(ETA_PH, calc_ph_area(sLat, sLong), irrTiltZacatecas))
    
    ####
    print("\n2. Maximum power output for curved photovoltaic area:")
    # For dresden at 50°Latitude
    pDresden21June = power_eff_total_curve(bDiameter, sLat, sLong, ETA_PH, OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_50Lat, I_GLOB_MAX, GEO_HEIGHT_DRESDEN, TURBIDITY_DRESDEN)
    print("\nDresden, Germany, 112m geodetic height, solar elevation 62,4°:")
    print_tuples(pDresden21June)
    print("     Total effective power: %f" % sum_power_strips(pDresden21June, 5))
    # Total power for straight area under perpendicular angle: 
    
    """
    # Assuming settings of photovoltaic and airship stay same.
    # Picking a place close to equator around 0°Latitude, such as Ecuador or Colombia.
    # This region will probably have the highest sun elevation points in spring or autumn (21.3. or 23.9.)
    # Optional one could use mexico at 23°Latitude in summer (21.6.).
    # For this see the calculation on max. elevation angles.
    
    # There is a potential BUG!
    # The solar constant here (extracted from the works of Dr. Andreas Gassel) are averaged.
    # Normally the values on the southern hemisphere tend to be slightly higher then on the northern
    # hemisphere. This introduces inaccuracies...
    
    """
    # Same sun conditions in Ecuador but with geodestic height of Dresden, 12 p.m., solar elevation 90°
    pED21June = power_eff_total_curve(bDiameter, sLat, sLong, ETA_PH, OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_0Lat, I_GLOB_MAX, GEO_HEIGHT_DRESDEN, TURBIDITY_DRESDEN)
    print("\nAt equator, geodestical height of Dresden, solar elevation 90°:")
    print_tuples(pED21June)
    print("     Total effective power: %f" % sum_power_strips(pED21June, 5))
    
    # Same sun conditions in Ecuador but with geodestic height of Dresden, solar elevation 62.4°
    pED21June624 = power_eff_total_curve(bDiameter, sLat, sLong, ETA_PH, OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_50Lat, I_GLOB_MAX, GEO_HEIGHT_DRESDEN, TURBIDITY_DRESDEN)
    print("\nAt equator, geodestical height of Dresden, solar elevation 62.4°:")
    print_tuples(pED21June624)
    print("     Total effective power: %f" % sum_power_strips(pED21June624, 5))
    
    # Ecuador, Santo Domingo, ~0°Lat, 655m geodestical height, 12 p.m.
    pSantoDomingo21June = power_eff_total_curve(bDiameter, sLat, sLong, ETA_PH, OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_0Lat, I_GLOB_MAX, GEO_HEIGHT_SANTODOMINGO, TURBIDITY_DRESDEN)
    print("\nEcuador, Santo Domingo, ~0°Lat, 655m geodestical height, solar elevation 90°:")
    print_tuples(pSantoDomingo21June)
    print("     Total effective power: %f" % sum_power_strips(pSantoDomingo21June, 5))
    
    # Ecuador, Quito, ~0°Lat, 2850m geodestic height, 12 p.m.
    pQuito21June = power_eff_total_curve(bDiameter, sLat, sLong, ETA_PH, OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_0Lat, I_GLOB_MAX, GEO_HEIGHT_QUITO, TURBIDITY_DRESDEN)
    print("\nEcuador, Quito, ~0°Lat, 2850m geodestic height, solar elevation 90°:")
    print_tuples(pSantoDomingo21June)
    print("     Total effective power: %f" % sum_power_strips(pQuito21June, 5))  
    
    # Mexiko, Zacatecas, 23°Lat (norther tropic), 2440m geodestic height, 12 p.m.
    pZacateras21June = power_eff_total_curve(bDiameter, sLat, sLong, ETA_PH, OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_0Lat, I_GLOB_MAX, GEO_HEIGHT_ZACATECAS, TURBIDITY_DRESDEN)
    print("\nMexiko, Zacatecas, 23°Lat (norther tropic), 2440m geodestic height, solar elevation 90°:")
    print_tuples(pED21June)
    print("     Total effective power: %f" % sum_power_strips(pED21June, 5))
    
    # Mexiko, , 23°Lat (norther tropic), geodestic height
    
    ####
    ## Maybe create csv file for data log on each execution
    ####


# Main
if __name__ == "__main__":
    print("Run basic testing...")
    test__all()
    print()
    
    I_T_For_Tilt(tilt_angle(90.0), OP_DAY_MID_SUMMER, MAX_SOLAR_ELEVATION_0Lat, I_GLOB_MAX, GEO_HEIGHT_SANTODOMINGO, TURBIDITY_DRESDEN)
    
    print("\n\n\n")
    print("Run simulation.")
    sim()
    
    print("Max. solar elevations per latitude:")
    print("Summer:")
    print_solar_elevations(1)
    print("Winter:")
    print_solar_elevations(3)
    