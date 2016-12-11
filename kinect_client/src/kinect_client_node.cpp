#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <string.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <netdb.h> 
#include <string.h>
#include <std_msgs/String.h>
#include <ros/ros.h>


void error(const char *msg)
{
    perror(msg);
    exit(0);
}

int main(int argc, char *argv[])
{
    /*
    Basic Socket Stuff to enable the connection
    */
    int sockfd, portno, n;
    struct sockaddr_in serv_addr;
    struct hostent *server;

    char buffer_read[256];
    char buffer_write[256];
    if (argc < 3) {
       fprintf(stderr,"usage %s hostname port\n", argv[0]);
       exit(0);
    }
    portno = atoi(argv[2]);
    sockfd = socket(AF_INET, SOCK_STREAM, 0);
    if (sockfd < 0) 
        error("ERROR opening socket");
    server = gethostbyname(argv[1]);
    if (server == NULL) {
        fprintf(stderr,"ERROR, no such host\n");
        exit(0);
    }
    bzero((char *) &serv_addr, sizeof(serv_addr));
    serv_addr.sin_family = AF_INET;
    bcopy((char *)server->h_addr, 
         (char *)&serv_addr.sin_addr.s_addr,
         server->h_length);
    serv_addr.sin_port = htons(portno);
    if (connect(sockfd,(struct sockaddr *) &serv_addr,sizeof(serv_addr)) < 0) 
        error("ERROR connecting");
    /*
    Prepare for using ROS

    */
    ros::init(argc, argv, "talker");
    ros::NodeHandle talker_node;
    ros::Publisher chatter_pub = talker_node.advertise<std_msgs::String>("chatter", 1000);

    //ros::Rate loop_rate(10);

    /*
    Here its about writing some crazy stuff
    still sending messages may be useless?!
    */
    std::string tmp = "gimme something";
    while (ros::ok()) {
        //Communication with the Server
        //printf("Please enter the message: ");
        //bzero(buffer_write,256);
        //fgets(buffer_write,255,stdin);

        strncpy(buffer_write, tmp.c_str(), sizeof(buffer_write));
        n = write(sockfd,buffer_write,strlen(buffer_write));
        if (n < 0) 
            error("ERROR writing to socket");
        bzero(buffer_read ,256);
        n = read(sockfd,buffer_read,255);
        if (n < 0) 
            error("ERROR reading from socket");
        printf("%s\n",buffer_read);

        //this is about publishing my message
        std_msgs::String msg;
        std::stringstream ss;

        ss << buffer_read;
        msg.data = ss.str();

        ROS_INFO("%s", msg.data.c_str());

        chatter_pub.publish(msg);
        //ros::spinOnce();


    }

    close(sockfd);
    return 0;
}