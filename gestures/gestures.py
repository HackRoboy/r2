## ================================== ##
##  Read in measured Roboy movements  ##
## ================================== ##

## This document takes the inputs of moving Roboy's arm along with all the recordable data from the motors, spring, joint and perhaps the Kinect.
## It should then find a function to map between movements and data, so that we can then provide inputs (so number of motor rotations and duration of motor activity etc.)

import numpy as np
import matplotlib.pyplot as plt

## Create names for each file for each of the gestures
## DIRTY is the measured input
## CLEAN contains only the joints and positions over time
## OUT is the start and end positions for each of the gestures
HANDSHAKE_DIRTY = "measured_handshake.txt"
HANDSHAKE_CLEAN = "clean_handshake.txt"
HANDSHAKE_OUT = "handshake_command.txt"  # change the output format of this?

HUG_DIRTY = "measured_hug.txt"
HUG_CLEAN = "clean_hug.txt"
HUG_OUT = "hug_command.txt"  # change the output format of this?

WAVE_DIRTY = "measured_wave.txt"
WAVE_CLEAN = "clean_wave.txt"
WAVE_OUT = "wave_command.txt"  # change the output format of this?

HIGHFIVE_DIRTY = "measured_highfive.txt"
HIGHFIVE_CLEAN = "clean_highfive.txt"
HIGHFIVE_OUT = "highfive_command.txt"  # change the output format of this?

## Function to read in measured data for each gesture
def cleaner(input_file, output_file):
    """Take the measured movements of each of the joints (in radians) and \
    write only the joint-position tuples into a new file"""
    with open(input_file, 'r') as old:
        with open(output_file, 'w') as new:
            for line in old:
                if line.split(":")[0] == "position":
                    #print line + "\n"
                    new.write(line.split(":")[1][2:-2] + "\n")
    return None

## ================================== ##
##  Extract start and stop positions  ##
## ================================== ##

def create_gesture_array(gesture_clean, create_ofile=False):
    """Create three outputs: start, end, delta of all recorded movements"""
    data = np.genfromtxt(gesture_clean, delimiter=",")

    start = data[0]
    end = data[-1]
    delta = end - start

    if create_ofile == True:
        with open(str(gesture_clean[:-4]) + "_out.txt", 'w') as ofile:
            ofile.write(start)
            ofile.write(end)
            ofile.write(delta)
            
    return start, end, delta




## ================================================= ##
##  Plot measured movements of each joint over time  ##
## ================================================= ##

input_file = "clean_handshake_data.txt" # CSV with only joint measurements

def plot_movements(input_file):
    
    data = np.genfromtxt(input_file, delimiter=",")

    plt.plot(data)
    plt.legend(loc="upper left")
    plt.xlabel('Measure cycle')
    plt.ylabel('Movement in radians')
    plt.title(r'$\mathbf{Handshake}$' + ' - movement of each joint over time')
    plt.legend(loc="best")
    plt.show()
    return None    

plot_movements(input_file)


## ========== ##
##  Run file  ##
## ========== ##

# def main():
    
# if __name__ == '__main__':
#     main(
#         ## Run for handshakes
        

#         ## Run for hug


#         ## Run for high-five


#         ## Run for wave

        
#     )
