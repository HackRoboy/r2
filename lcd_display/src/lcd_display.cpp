#include "ros/ros.h"
#include "std_msgs/String.h"
#include <jhd1313m1.hpp>

void displayCallback(const std_msgs::String::ConstPtr& msg)
{
  ROS_INFO("I heard: [%s]", msg->data.c_str());
  upm::Jhd1313m1 *lcd = new upm::Jhd1313m1(0, 0x3E, 0x62);
  lcd->setCursor(0,0);
  lcd->write(msg->data.c_str());
  sleep(5);
  delete lcd;

}

int main(int argc, char **argv)
{
  ros::init(argc, argv, "lcd_display");

  ros::NodeHandle n;

  ros::Subscriber sub = n.subscribe("lcd", 1000, displayCallback);

  upm::Jhd1313m1 *lcd = new upm::Jhd1313m1(0, 0x3E, 0x62);
  lcd->setCursor(0,0);
  lcd->write("I Roboy");
  sleep(5);
  delete lcd;

  ros::spin();

  return 0;
}
