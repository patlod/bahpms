#!/usr/bin/env python

import math

"""
This script calculates the charge iterations based on the numeric model for TWO BATTERIES used in Section "Flight Time Enhancement"
"""

# Parameters
SOC_MAX = 2400
RATE_C_DEFAULT = 2.5
RATE_C_PV_CURVED_DRESDEN = 2.2
RATE_D_SET1 = 2.84
RATE_D_SET2 = 6.32
RATE_D_SET3 = 2.2
RATE_D_SET4 = 4.75


def t_D(i, SOC_max, SOC_i, Rate_D):
   return ( 1 / Rate_D) * 60 if (i == 0) else (SOC_i / (SOC_max * Rate_D)) * 60

def t_C(Rate_C):
    return (1 / Rate_C) * 60
    
def C_FF(i, t_D, t_C):
    if (i == 0):
        return 1
    else:
        t_div = (t_D / t_C)
        if t_div >= 1:
            return 1
        else:
            return t_div

def SOC(i, c_ff, SOC_max):
    return SOC_max if (i == 0) else c_ff * SOC_max



    

# A better implementation would probably be to dynamically calculate the data on the array i.e.
# calculate current step based on data from step before..
def calc_TFT(iterations, SOC_max, Rate_D, Rate_C):
    """
    Computes total flight time in min and returns all charge iterations for two batteries with given battery capacity at discharge rate and charge rate.
    """
    # Charge time to reach SOC_max stays constant
    t_C_const = t_C(Rate_C)
    
    # Arrays of format: [iteration, t_D_i, t_C_i, SOC_i, C_FF_i, t_D_total]
    iteration_steps = []
    
    for i in range(0, iterations):

        if i == 0:
            t_D_0 = ( 1 / Rate_D ) * 60
            step = [ -1,
                     t_D_0,
                     t_C_const,
                     SOC_max,
                     C_FF(0, 0, 0),
                     t_D_0
            ]
            iteration_steps.append(step)
            continue
        
        i2nd = i - 1
        if i == 1:
            t_D_0 = iteration_steps[i-1][1]
            step = [    i2nd,
                        t_D_0,
                        t_C_const,
                        SOC_max,
                        C_FF(0, 0, 0),
                        iteration_steps[i-1][5] + t_D_0 ]
            iteration_steps.append(step)
            continue
        
        # Order!
        c_ff = C_FF(i2nd, iteration_steps[i-1][1], t_C_const)
        soc = SOC(i2nd, c_ff, SOC_max)

        t_D_i = t_D(i2nd, SOC_max, soc, Rate_D)
        t_D_total = iteration_steps[i-1][5] + t_D_i
        cur_step = [ i2nd,
                     t_D_i, 
                     t_C_const, 
                     soc, 
                     c_ff,
                     t_D_total ]
        iteration_steps.append(cur_step)

    return iteration_steps

    
    
def print_latex_table(data):
    for d in data:
        if d[0] < 0:
            print("-- & D & F & %f & %f & %f & %f & %f \\" % (d[1],d[2],d[3],d[4],d[5]) )
        else:
            if d[0] % 2 == 0:
                print("%d & C & D & %f & %f & %f & %f & %f \\" % (d[0],d[1],d[2],d[3],d[4],d[5]) )
            else:
                print("%d & D & C & %f & %f & %f & %f & %f \\" % (d[0],d[1],d[2],d[3],d[4],d[5]) )

def print_plot_data_TFT(data):
    s = ""
    for d in data:
        s += "(" + str(d[0]+1) + "," + str(d[5]) + ")"
    print(s)
    
def main():
    # FTE for Settings 1, 2, 3, 4 with default charge rate of 2,5C
    print("Setting 1 -- Default:")
    default_set1 = calc_TFT(25, SOC_MAX, RATE_D_SET1, RATE_C_DEFAULT)
    print_latex_table(default_set1)
    print_plot_data_TFT(default_set1)
    print()
    
    print("Setting 2 -- Default:")    
    default_set2 = calc_TFT(25, SOC_MAX, RATE_D_SET2, RATE_C_DEFAULT)
    print_latex_table(default_set2)
    print_plot_data_TFT(default_set2)
    print()
    
    print("Setting 3 -- Default:")
    default_set3 = calc_TFT(25, SOC_MAX, RATE_D_SET3, RATE_C_DEFAULT)
    print_latex_table(default_set3)
    print_plot_data_TFT(default_set3)
    print()
    
    print("Setting 4 -- Default:")
    default_set4 = calc_TFT(25, SOC_MAX, RATE_D_SET4, RATE_C_DEFAULT)
    print_latex_table(default_set4)
    print_plot_data_TFT(default_set4)
    print()
    
    # FTE for Settings 1, 2, 3, 4 with default charge rate of 2,5C
    print("Setting 1 -- PV curved:")
    pv_curved_set1 = calc_TFT(25, SOC_MAX, RATE_D_SET1, RATE_C_PV_CURVED_DRESDEN)
    print_latex_table(pv_curved_set1)
    print_plot_data_TFT(pv_curved_set1)
    print()
    
    print("Setting 2 -- PV curved:")
    pv_curved_set2 = calc_TFT(25, SOC_MAX, RATE_D_SET2, RATE_C_PV_CURVED_DRESDEN)
    print_latex_table(pv_curved_set2)
    print_plot_data_TFT(pv_curved_set2)
    print()
    
    print("Setting 3 -- PV curved:")
    pv_curved_set3 = calc_TFT(25, SOC_MAX, RATE_D_SET3, RATE_C_PV_CURVED_DRESDEN)
    print_latex_table(pv_curved_set3)
    print_plot_data_TFT(pv_curved_set3)
    print()
    
    print("Setting 4 -- PV curved:")
    pv_curved_set4 = calc_TFT(25, SOC_MAX, RATE_D_SET4, RATE_C_PV_CURVED_DRESDEN)
    print_latex_table(pv_curved_set4)
    print_plot_data_TFT(pv_curved_set4)
    print()

if __name__ == "__main__":
    main()