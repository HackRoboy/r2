#include "ros/ros.h"
#include "std_msgs/String.h"
#include "std_msgs/Float64MultiArray.h"
#include <iostream>
#include "rosconsole/macros_generated.h"
#include <sstream>

using namespace std;

#define size  43

//topics names
//=============================
string KINETIK_TOPIC="chatter";
string MOTION_CMD_TOPIC="/motion/command";
string MOTION_FDBK_TOPIC="/motion/feedback";

//========================
//COMMANDS
//==============
string HS="HS";// hand shake command
string RHS="RHS";// reverse hand shake command
string HF="HF";// high five command
string RHF="RHF";//reverse high five command
string HUG="HUG";// Hug command
string RHUG="RHUG";// referseHug command
string WV="WAVE";// wave command
string RWV="RWAVE";// wave command

string DONE_MESSAGE="DONE";
string FAIL_MESSAGE="FAIL";
string BACK_TO_DEFAULT="default";


/**
 * This tutorial demonstrates simple sending of messages over the ROS system.
 */

ros::Publisher chatter_pub;

ros::Publisher fdbk_pub;
//int size = 43 ;
float dataOld[size];

void cmdCallback(const std_msgs::String::ConstPtr& msg)
{
	string cmd=msg->data.c_str();
	std_msgs::String fdbk;
	if(cmd==HS){
	//define arrays and send data function
	fdbk.data=DONE_MESSAGE;
	fdbk_pub.publish(fdbk);
	}
	if(cmd==RHS){
	
	fdbk.data=BACK_TO_DEFAULT;
	fdbk_pub.publish(fdbk);
	}
	if(cmd==HF){
	
	fdbk.data=DONE_MESSAGE;
	fdbk_pub.publish(fdbk);
	}
	if(cmd==RHF){
	
	fdbk.data=BACK_TO_DEFAULT;
	fdbk_pub.publish(fdbk);
	}
	if(cmd==HUG){
	
	fdbk.data=DONE_MESSAGE;
	fdbk_pub.publish(fdbk);
	}
	if(cmd==RHUG){
	
	fdbk.data=BACK_TO_DEFAULT;
	fdbk_pub.publish(fdbk);
	}
	if(cmd==WV){
	

	fdbk.data=DONE_MESSAGE;
	fdbk_pub.publish(fdbk);
	}
	if(cmd==RWV){
	
	fdbk.data=BACK_TO_DEFAULT;
	fdbk_pub.publish(fdbk);
	}
}

void sendArray(int n,int index[],float values[] ) {

    std_msgs::Float64MultiArray msg;
    int k = 0;
    for (int i = 0;i < size ; i++) {
      if (i != index[k]) {
        msg.data.push_back(dataOld[i]);
       // ROS_INFO("data.push[%d]=%fl\n", i,dataOld[i]);

    	}
      else {
        msg.data.push_back(values[k]);
        //ROS_INFO("data.push[%d]=%fl\n", i,values[k]);
        dataOld[i] = values[k];
	if(k<n-1)
        	k++;
      }

    }
    

    ROS_INFO("%s", "in");

    //msg.data[5];

    chatter_pub.publish(msg);
}

int main(int argc, char **argv)
{
  ros::init(argc, argv, "motion_node");
  while(ros::ok()){
  for (int i = 0; i < size ; i++) {
    dataOld[i] = 0;
  }
  ros::NodeHandle n;
  ros::Subscriber sub = n.subscribe(MOTION_CMD_TOPIC, 1000, cmdCallback);
  chatter_pub = n.advertise<std_msgs::Float64MultiArray>("/rrbot/joint_position_controller/command", 1000);
  ros::Publisher chatter_pub_test = n.advertise<std_msgs::String>("/test", 1000);
  fdbk_pub = n.advertise<std_msgs::String>(MOTION_FDBK_TOPIC, 1000);
	std_msgs::String msg_test;
	msg_test.data = "good morning";
	chatter_pub_test.publish(msg_test);
  int num=size;
  int index[size];
  for(int i=0;i<size;i++)
     index[i]=i;
  float values[]={10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10,20,10};
  sendArray(num,index,values);



  


  //ros::Subscriber sub = n.subscribe("/update_values", 1000, chatterCallback);

  //l will be loop for the whole array,
  //k will save which joint we are going to work
  //i will be the iterator for the foor loop
  

    ROS_INFO("out");

    //msg.data[5];

    ros::spinOnce();
  }


  return 0;
}
