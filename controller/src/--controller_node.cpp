//Includes all the headers necessary to use the most common public pieces of the ROS system.
#include <ros/ros.h>
#include <ctime>
#include "std_msgs/String.h"
using namespace std;

//topics names
//=============================
string KINETIK_TOPIC="";
string MOTION_CMD_TOPIC="";
string MOTION_FDBK_TOPIC="";

//========================
//COMMANDS
//==============
string HS="HS";// hand shake command
string HF="HF";// high five command
string HUG="HUG";// Hug command
string WV="WAVE";// wave command

string DONE_MESSAGE="DONE";
string FAIL_MESSAGE="FAIL";
string BACK_TO_DEFAULT="default";
double CMD_REVERT_DURATION=5;
//============

bool idle=true;
ros::Publisher cmd_pub;


//time stuff
//=====================
clock_t lastCmdTime,lastFdbkTime;
string lastSentCmd="";
string lastReceivedCmd="";

//==========

//Parameters
//============================================
//============================================


//============================================
//============================================

bool getState(){

double duration = ( clock() - lastFdbkTime ) / (double) CLOCKS_PER_SEC;
return duration > CMD_REVERT_DURATION;

	
}
//This function is called everytime a new command is published
void chatterCallback(const std_msgs::String::ConstPtr& msg)
{
	lastReceivedCmd=msg->data.c_str();;
	idle=false?getState():idle;
        // handle the command
        if(idle){
		
		cmd_pub.publish(msg);
		lastSentCmd=msg->data.c_str();
		idle=false;
		lastFdbkTime=(double)DBL_MAX;// to make sure that getstate returns true only when feedback time is implicitly changed
		lastCmdTime=clock();
		
		
	}
}

void feedbackCallback(const std_msgs::String::ConstPtr& msg)
{
	string message=msg->data.c_str();
        if(message==DONE_MESSAGE){

		lastFdbkTime=clock();
		double duration=0;
		double compareTime=CMD_REVERT_DURATION;
		while(duration<compareTime){
			duration = ( clock() - lastFdbkTime ) / (double) CLOCKS_PER_SEC;
			//instead if using sleep because sleep may sometimes sleep the node not the process
                        if(duration>CMD_REVERT_DURATION-1 && lastReceivedCmd==lastSentCmd)
				compareTime++;
		}
		lastSentCmd.insert(0,1,'R');
		std_msgs::String rmsg;
		rmsg.data=lastSentCmd;
		cmd_pub.publish(rmsg);
	}
	
	if(message==BACK_TO_DEFAULT){

		idle=true;
	}

	if(message==FAIL_MESSAGE){

		idle=true;
	}
}
 
/**
* This tutorial demonstrates simple image conversion between ROS image message and OpenCV formats and image processing
*/
int main(int argc, char **argv)
{

    ros::init(argc, argv, "image_processor");
    ros::NodeHandle n;
    
   

    /**
    * Subscribe to the "camera/image_raw" base topic. The actual ROS topic subscribed to depends on which transport is used.
    * In the default case, "raw" transport, the topic is in fact "camera/image_raw" with type sensor_msgs/Image. ROS will call
    * the "imageCallback" function whenever a new image arrives. The 2nd argument is the queue size.
    * subscribe() returns an image_transport::Subscriber object, that you must hold on to until you want to unsubscribe.
    * When the Subscriber object is destructed, it will automatically unsubscribe from the "camera/image_raw" base topic.
    */
        ros::Subscriber sub = n.subscribe(KINETIK_TOPIC, 1000, chatterCallback);
	ros::Subscriber fdbk_sub = n.subscribe(MOTION_FDBK_TOPIC, 1000, feedbackCallback);

	cmd_pub  = n.advertise<std_msgs::String>(MOTION_CMD_TOPIC, 1000);
	
    /**
    * In this application all user callbacks will be called from within the ros::spin() call.
    * ros::spin() will not return until the node has been shutdown, either through a call
    * to ros::shutdown() or a Ctrl-C.
    */
        ros::spin();
 
}

