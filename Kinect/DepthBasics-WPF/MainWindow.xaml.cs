//------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.DepthBasics
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Microsoft.Kinect;
    using Microsoft.Kinect.Toolkit;
    using Microsoft.Kinect.Toolkit.Controls;
    using Microsoft.Kinect.Toolkit.Interaction;
    using System.Text;
    using System.Collections.Generic;
    using Microsoft.Speech.AudioFormat;
    using Microsoft.Speech.Recognition;
    using System.Windows.Documents;
    using System.Net.Sockets;
    using System.Net;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable",
        Justification = "In a full-fledged application, the SpeechRecognitionEngine object should be properly disposed. For the sake of simplicity, we're omitting that code in this sample.")]
    
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensor sensor;
        #region Properties
        /// <summary>
        /// Bitmap that will hold color information
        /// </summary>
        private WriteableBitmap colorBitmap;

        /// <summary>
        /// Intermediate storage for the depth data received from the camera
        /// </summary>
        private DepthImagePixel[] depthPixels;

        /// <summary>
        /// Intermediate storage for the depth data converted to color
        /// </summary>
        private byte[] colorPixels;

        /// <summary>
        /// Width of output drawing
        /// </summary>
        private const float RenderWidth = 640.0f;

        /// <summary>
        /// Height of our output drawing
        /// </summary>
        private const float RenderHeight = 480.0f;

        /// <summary>
        /// Thickness of drawn joint lines
        /// </summary>
        private const double JointThickness = 3;

        /// <summary>
        /// Thickness of body center ellipse
        /// </summary>
        private const double BodyCenterThickness = 10;

        /// <summary>
        /// Thickness of clip edge rectangles
        /// </summary>
        private const double ClipBoundsThickness = 10;

        /// <summary>
        /// Brush used to draw skeleton center point
        /// </summary>
        private readonly Brush centerPointBrush = Brushes.Blue;

        /// <summary>
        /// Brush used for drawing joints that are currently tracked
        /// </summary>
        private readonly Brush trackedJointBrush = new SolidColorBrush(Color.FromArgb(255, 68, 192, 68));

        /// <summary>
        /// Brush used for drawing joints that are currently inferred
        /// </summary>        
        private readonly Brush inferredJointBrush = Brushes.Yellow;

        /// <summary>
        /// Pen used for drawing bones that are currently tracked
        /// </summary>
        private readonly Pen trackedBonePen = new Pen(Brushes.Green, 6);

        /// <summary>
        /// Pen used for drawing bones that are currently inferred
        /// </summary>        
        private readonly Pen inferredBonePen = new Pen(Brushes.Gray, 1);

        /// <summary>
        /// Drawing group for skeleton rendering output
        /// </summary>
        private DrawingGroup drawingGroup;

        /// <summary>
        /// Drawing image that we will display
        /// </summary>
        private DrawingImage imageSource;

        private InteractionStream _interactionStream;
        private UserInfo[] _userInfos; //the information about the interactive users


        /// <summary>
        /// Speech recognition engine using audio data from Kinect.
        /// </summary>
        private SpeechRecognitionEngine speechEngine;

        #endregion


        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }


        

        /// <summary>
        /// Execute startup tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

            tcpclnt = new TcpClient();
            try
            {
                System.Net.IPAddress ipAd = IPAddress.Parse("138.246.13.102");

                myList = new TcpListener(ipAd, 8001);


                myList.Start();


                s = myList.AcceptSocket();
                Button.Visibility=Visibility.Hidden;
            }
            catch (Exception) { //MessageBox.Show("Connection failed"); 
            };

            // Create the drawing group we'll use for drawing
            this.drawingGroup = new DrawingGroup();

            // Create an image source that we can use in our image control
            this.imageSource = new DrawingImage(this.drawingGroup);

            // Display the drawing using our image control
            ImageSK.Source = this.imageSource;

            // Look through all sensors and start the first connected one.
            // This requires that a Kinect is connected at the time of app startup.
            // To make your app robust against plug/unplug, 
            // it is recommended to use KinectSensorChooser provided in Microsoft.Kinect.Toolkit (See components in Toolkit Browser).
            foreach (var potentialSensor in KinectSensor.KinectSensors)
            {
                if (potentialSensor.Status == KinectStatus.Connected)
                {
                    this.sensor = potentialSensor;
                    break;
                }
            }

            if (null != this.sensor)
            {
                // Turn on interaction stream
                //this.sensor.
                // Turn on the skeleton stream to receive skeleton frames
                this.sensor.SkeletonStream.Enable();

                // Add an event handler to be called whenever there is new color frame data
                this.sensor.SkeletonFrameReady += this.SensorSkeletonFrameReady;

                // Turn on the depth stream to receive depth frames
                this.sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                
                // Allocate space to put the depth pixels we'll receive
                this.depthPixels = new DepthImagePixel[this.sensor.DepthStream.FramePixelDataLength];

                // Allocate space to put the color pixels we'll create
                this.colorPixels = new byte[this.sensor.DepthStream.FramePixelDataLength * sizeof(int)];

                // This is the bitmap we'll display on-screen
                this.colorBitmap = new WriteableBitmap(this.sensor.DepthStream.FrameWidth, this.sensor.DepthStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

                // Set the image we display to point to the bitmap where we'll put the image data
                this.Image.Source = this.colorBitmap;

                // Add an event handler to be called whenever there is new depth frame data
                this.sensor.DepthFrameReady += this.SensorDepthFrameReady;

              //  this.sensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;

                try
                {
                    _userInfos = new UserInfo[InteractionFrame.UserInfoArrayLength];
                    _interactionStream = new InteractionStream(sensor, new InteractionClient());
                    _interactionStream.InteractionFrameReady += InteractionStreamOnInteractionFrameReady;
                }
                catch (Exception)
                {
                    MessageBox.Show("kinect");
                }



                // Start the sensor!
                try
                {
                    this.sensor.Start();
                }
                catch (IOException)
                {
                    this.sensor = null;
                }


               // kinectRegion.KinectSensor = sensor;
            }

            if (null == this.sensor)
            {
                this.statusBarText.Text = Properties.Resources.NoKinectReady;
            }


            RecognizerInfo ri = GetKinectRecognizer();

            if (null != ri)
            {

                this.speechEngine = new SpeechRecognitionEngine(ri.Id);

                using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.SpeechGrammar)))
                {
                    var g = new Grammar(memoryStream);
                    speechEngine.LoadGrammar(g);
                }

                speechEngine.SpeechRecognized += SpeechRecognized;
                //speechEngine.SpeechRecognitionRejected += SpeechRejected;

                try
                {
                    speechEngine.SetInputToAudioStream(
                        sensor.AudioSource.Start(), new SpeechAudioFormatInfo(EncodingFormat.Pcm, 16000, 16, 1, 32000, 2, null));
                    speechEngine.RecognizeAsync(RecognizeMode.Multiple);
                }
                catch (Exception) { }
            }

        }


        #region TCP
        
        TcpClient tcpclnt;
        Socket s;
        TcpListener myList;

        private void sendMessage(string str)
        {
            //String str = s;
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(str);
            try
            {
                //Stream stm = tcpclnt.GetStream();
                //stm.Write(ba, 0, ba.Length);
                this.s.Send(ba);
            }
            catch (Exception) { //MessageBox.Show("Connection failed");

                //myList.Stop();
                //myList.Start();
                //s = myList.AcceptSocket();
            };
        }

        private string readMessage()
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba2 = new byte[256];
            //Stream stm = tcpclnt.GetStream();
            //stm.Read(ba2, 0, 256);
            try
            {
                s.Receive(ba2);
            }
            catch (Exception) { }
            return asen.GetString(ba2);

        }

        /*private void sendMessage(string s)
        {
            String str = s;
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(str);
            try
            {
                Stream stm = tcpclnt.GetStream();
                stm.Write(ba, 0, ba.Length);
            }
            catch (Exception) { MessageBox.Show("Connection failed"); };
        }
        private string readMessage()
        {
            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba2 = new byte[256];
            Stream stm = tcpclnt.GetStream();
            stm.Read(ba2, 0, 256);
            return asen.GetString(ba2);

        }
          */
        #endregion

        #region Speech
        /// <summary>
        /// Gets the metadata for the speech recognizer (acoustic model) most suitable to
        /// process audio from Kinect device.
        /// </summary>
        /// <returns>
        /// RecognizerInfo if found, <code>null</code> otherwise.
        /// </returns>
        private static RecognizerInfo GetKinectRecognizer()
        {
            foreach (RecognizerInfo recognizer in SpeechRecognitionEngine.InstalledRecognizers())
            {
                string value;
                recognizer.AdditionalInfo.TryGetValue("Kinect", out value);
                if ("True".Equals(value, StringComparison.OrdinalIgnoreCase) && "en-US".Equals(recognizer.Culture.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return recognizer;
                }
            }

            return null;
        }

        /// <summary>
        /// Handler for recognized speech events.
        /// </summary>
        /// <param name="sender">object sending the event.</param>
        /// <param name="e">event arguments.</param>
        private void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            // Speech utterance confidence below which we treat speech as if it hadn't been heard
            const double ConfidenceThreshold = 0.4;


            if (e.Result.Confidence >= ConfidenceThreshold)
            {
                string s;
                string[] str;
                switch (e.Result.Semantics.Value.ToString())
                {
                    /*case "MOVE":
                        //MessageBox.Show(X.Content + "\n" + Y.Content + "\n" + Z.Content);
                        readMessage();
                        sendMessage("cmd 1\n");
                        s = readMessage();
                        try
                        {
                            str = s.Split('[');
                            str = str[1].Split(']');
                            str = str[0].Split(',');

                            sendMessage("received\n");
                            readMessage();
                            sendMessage("cmd 3\n");
                            readMessage();
                            sendMessage("(" + gx + "," + gy + "," + gz + "," + str[3] + "," + str[4] + "," + str[5] + ")\n");
                            //sendMessage(X.Content + "\n" + Y.Content + "\n" + Z.Content + " \n\n");
                        }
                        catch(Exception){
                            sendMessage("error\n");
                        }
                        break;

                    case "DOWN":
                        readMessage();
                        sendMessage("cmd 4\n");
                        readMessage();
                        sendMessage("dir 0\n");
                        
                        Orientation.Content = "Orientation:  Down";

                        break;

                    case "LEFT":

                        readMessage();
                        sendMessage("cmd 4\n");
                        readMessage();
                        sendMessage("dir 1\n");
                       
                        
                        Orientation.Content = "Orientation:  Left";
                        break;

                    case "RIGHT":

                        readMessage();
                        sendMessage("cmd 4\n");
                        readMessage();
                        sendMessage("dir 2\n");
                        
                       // sendMessage("movej(p[" + str[0] + "," + str[1] + "," + str[2] + "," +str[3]+","+str[4]+","+str[5]+"])\n");
                        Orientation.Content = "Orientation: Right";
                        break;
                     */
                    case "HS":
                        readMessage();
                        sendMessage("hand shake\n");
                        break;
                    case "HF":
                        readMessage();
                        sendMessage("high five\n");
                        break;
                    case "WAVE":
                        readMessage();
                        sendMessage("wave\n");
                        break;
                    case "HUG":
                        readMessage();
                        sendMessage("hug\n");
                        break;
                }
            }
        }
        #endregion


        #region Depth&Skeleton


        private double handZ, hipZ,ox, oy, oz;//, x, y, z, ax, ay, az,gx,gy,gz;
        private MySkelet mySkeleton=new MySkelet();
        private int averageCount = 0;
        private double getJointThickness = 3;
        
        
        /// <summary>
        /// Draws indicators to show which edges are clipping skeleton data
        /// </summary>
        /// <param name="skeleton">skeleton to draw clipping information for</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private static void RenderClippedEdges(Skeleton skeleton, DrawingContext drawingContext)
        {
            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Bottom))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, RenderHeight - ClipBoundsThickness, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Top))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, RenderWidth, ClipBoundsThickness));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Left))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(0, 0, ClipBoundsThickness, RenderHeight));
            }

            if (skeleton.ClippedEdges.HasFlag(FrameEdges.Right))
            {
                drawingContext.DrawRectangle(
                    Brushes.Red,
                    null,
                    new Rect(RenderWidth - ClipBoundsThickness, 0, ClipBoundsThickness, RenderHeight));
            }
        }


        private Dictionary<int, InteractionHandEventType> _lastLeftHandEvents = new Dictionary<int, InteractionHandEventType>();
        private Dictionary<int, InteractionHandEventType> _lastRightHandEvents = new Dictionary<int, InteractionHandEventType>();

        private void InteractionStreamOnInteractionFrameReady(object sender, InteractionFrameReadyEventArgs e)
        {
             using (var iaf = e.OpenInteractionFrame()) //dispose as soon as possible
        {
            if (iaf == null)
                return;
     
           iaf.CopyInteractionDataTo(_userInfos);
       }
     
        StringBuilder dump = new StringBuilder();
     
        var hasUser = false;
        //statusBarText.Text = _userInfos[0].SkeletonTrackingId.ToString() + " " + _userInfos[1].SkeletonTrackingId.ToString() + " " + _userInfos[2].SkeletonTrackingId.ToString();
        foreach (var userInfo in _userInfos)
        {
            var userID = userInfo.SkeletonTrackingId;
            
           if (userID == 0)
               continue;
    
          hasUser = true;
           dump.AppendLine("User ID = " + userID);
            dump.AppendLine("  Hands: ");
            var hands = userInfo.HandPointers;
            if (hands.Count == 0)
                dump.AppendLine("    No hands");
            else
            {
                foreach (var hand in hands)
                {
                    var lastHandEvents = hand.HandType == InteractionHandType.Left
                                                ? _lastLeftHandEvents
                                                : _lastRightHandEvents;
     
                    if (hand.HandEventType != InteractionHandEventType.None)
                        lastHandEvents[userID] = hand.HandEventType;
     
                    var lastHandEvent = lastHandEvents.ContainsKey(userID)
                                            ? lastHandEvents[userID]
                                            : InteractionHandEventType.None;
     
                    dump.AppendLine();
                    dump.AppendLine("    HandType: " + hand.HandType);
                    dump.AppendLine("    HandEventType: " + hand.HandEventType);
                    dump.AppendLine("    LastHandEventType: " + lastHandEvent);
                    dump.AppendLine("    IsActive: " + hand.IsActive);
                    dump.AppendLine("    IsPrimaryForUser: " + hand.IsPrimaryForUser);
                    dump.AppendLine("    IsInteractive: " + hand.IsInteractive);
                    dump.AppendLine("    PressExtent: " + hand.PressExtent.ToString("N3"));
                    dump.AppendLine("    IsPressed: " + hand.IsPressed);
                   dump.AppendLine("    IsTracked: " + hand.IsTracked);
                    dump.AppendLine("    X: " + hand.X.ToString("N3"));
                    dump.AppendLine("    Y: " + hand.Y.ToString("N3"));
                    dump.AppendLine("    RawX: " + hand.RawX.ToString("N3"));
                    dump.AppendLine("    RawY: " + hand.RawY.ToString("N3"));
                    dump.AppendLine("    RawZ: " + hand.RawZ.ToString("N3"));
                    if (hand.HandType == InteractionHandType.Right)
                    {
                       // X.Content = "X: " + hand.X.ToString("N3");
                       // Y.Content = "Y: " + hand.Y.ToString("N3");
                       // Z.Content = "Z: " + (hand.RawZ*((handZ>hipZ)&&(hand.RawZ>0)?(-1):1)).ToString("N3");
                        if ((Event.Content.ToString()).CompareTo(lastHandEvent.ToString())!=0)
                        {
                            Event.Content = lastHandEvent;
                            if (Event.Content.ToString().CompareTo("Grip")==0)
                            {
                                //readMessage();
                                //sendMessage("cmd 6\n");
                                //sendMessage("set_digital_out(4,False)\n");
                                //sendMessage("set_digital_out(5,True)\n");
                            }
                            else
                                if (Event.Content.ToString().CompareTo("GripRelease") == 0)
                            {
                                //readMessage();
                                //sendMessage("cmd 5\n");
                            }
                        }
                        
                    }
                }
            }

            break;
        }

        if (!hasUser)
        {
            Event.Content = "No user detected.";
           // X.Content = "X: ";
           // Y.Content = "Y: ";
            //Z.Content = "Z: ";
        }
   }
        

        
        /// <summary>
        /// Event handler for Kinect sensor's SkeletonFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            Skeleton[] skeletons = new Skeleton[0];

            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);
                    try
            {
                
                var accelerometerReading = sensor.AccelerometerGetCurrentReading();
                _interactionStream.ProcessSkeleton(skeletons, accelerometerReading, skeletonFrame.Timestamp);
            }
           catch (InvalidOperationException)
            {
                // SkeletonFrame functions may throw when the sensor gets
                // into a bad state.  Ignore the frame in that case.
            }
                }
            }

            using (DrawingContext dc = this.drawingGroup.Open())
            {
                // Draw a transparent background to set the render size
                dc.DrawRectangle(Brushes.Transparent, null, new Rect(0.0, 0.0, RenderWidth, RenderHeight));

                if (skeletons.Length != 0)
                {
                    foreach (Skeleton skel in skeletons)
                    {
                        RenderClippedEdges(skel, dc);

                        if (skel.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            this.DrawBonesAndJoints(skel, dc);
                        }
                        else if (skel.TrackingState == SkeletonTrackingState.PositionOnly)
                        {
                            dc.DrawEllipse(
                            this.centerPointBrush,
                            null,
                            this.SkeletonPointToScreen(skel.Position),
                            BodyCenterThickness,
                            BodyCenterThickness);
                        }
                    }
                }

                // prevent drawing outside of our render area
                this.drawingGroup.ClipGeometry = new RectangleGeometry(new Rect(0.0, 0.0, RenderWidth, RenderHeight));
            }
        }

        /// <summary>
        /// Draws a skeleton's bones and joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        private void DrawBonesAndJoints(Skeleton skeleton, DrawingContext drawingContext)
        {
            // Render Torso
            this.DrawBone(skeleton, drawingContext, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.HipCenter);
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.HipCenter);
            //this.DrawBone(skeleton, drawingContext, JointType.ShoulderCenter, JointType.Spine);
            //this.DrawBone(skeleton, drawingContext, JointType.Spine, JointType.HipCenter);
            //this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipLeft);
            //this.DrawBone(skeleton, drawingContext, JointType.HipCenter, JointType.HipRight);

            // Left Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeleton, drawingContext, JointType.WristLeft, JointType.HandLeft);

            // Right Arm
            this.DrawBone(skeleton, drawingContext, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeleton, drawingContext, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeleton, drawingContext, JointType.WristRight, JointType.HandRight);

           /* // Left Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeleton, drawingContext, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleLeft, JointType.FootLeft);

            // Right Leg
            this.DrawBone(skeleton, drawingContext, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeleton, drawingContext, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeleton, drawingContext, JointType.AnkleRight, JointType.FootRight);*/

            // Render Joints
            foreach (Joint joint in skeleton.Joints)
            {
                Brush drawBrush = null;

                if (joint.TrackingState == JointTrackingState.Tracked)
                {
                    drawBrush = this.trackedJointBrush;
                }
                else if (joint.TrackingState == JointTrackingState.Inferred)
                {
                    drawBrush = this.inferredJointBrush;
                }

                if (drawBrush != null)
                {
                    if (joint.JointType == JointType.HandRight)
                    {
                        drawingContext.DrawEllipse(getBrushColor(), null, this.SkeletonPointToScreen(joint.Position), getJointThickness, getJointThickness);
                        handZ = joint.Position.Z;
                        mySkeleton.rHand.add(joint.Position.X, joint.Position.Y, joint.Position.Z);
                    }
                    if (joint.JointType == JointType.ElbowRight)
                    {
                        drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                        mySkeleton.rElbow.add(joint.Position.X, joint.Position.Y, joint.Position.Z);
                    }
                    if (joint.JointType == JointType.ShoulderRight)
                    {
                        drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                        mySkeleton.rShoulder.add(joint.Position.X, joint.Position.Y, joint.Position.Z);
                    }
                    if (joint.JointType == JointType.HandLeft)
                    {
                        drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                        mySkeleton.lHand.add(joint.Position.X, joint.Position.Y, joint.Position.Z);
                    }
                    if (joint.JointType == JointType.ElbowLeft)
                    {
                        drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                        mySkeleton.lElbow.add(joint.Position.X, joint.Position.Y, joint.Position.Z);
                    }

                    if (joint.JointType == JointType.ShoulderLeft)
                    {
                        drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                        mySkeleton.lShoulder.add(joint.Position.X, joint.Position.Y, joint.Position.Z);
                    }

                    if (joint.JointType == JointType.HipCenter)
                    {
                        drawingContext.DrawEllipse(drawBrush, null, this.SkeletonPointToScreen(joint.Position), JointThickness, JointThickness);
                        hipZ = joint.Position.Z;
                        ox = joint.Position.X;
                        oy = joint.Position.Y;
                        oz = joint.Position.Z;
                    }
                        
                    
                }
            }
            getJointThickness = 3 + Math.Pow(3, 5*Math.Abs(hipZ - handZ));
            averageCount++;
            if (averageCount == 10)
            {
                mySkeleton.average(averageCount);
               /* gx = ax / 10;
                gy = az / 10;
                gz = (-ay) / 10;*/
                X.Content = "";// "E-H: " + (mySkeleton.rElbow.z - mySkeleton.rHand.z);
                Y.Content = "";//"S-E: " + (mySkeleton.rShoulder.z - mySkeleton.rElbow.z);
                Z.Content = "";//"H-S: " + (mySkeleton.rHand.y - mySkeleton.rShoulder.y);
                averageCount = 0;
                string s=mySkeleton.checkMoves();
                switch (s)
                {
                    case "no":
                        break;
                    case "":
                        break;
                    default:
                        readMessage();
                        sendMessage(s);
                        break;

                }

            }
        }
        

        private Brush getBrushColor()
        {
            if(handZ>hipZ)
                return new SolidColorBrush(Color.FromArgb(255, 0,0,255));
            else
                return new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        }


        private bool usefulJoint(JointType jointType)
        {
            return jointType == JointType.WristRight || jointType == JointType.ElbowRight || jointType == JointType.HipCenter || jointType == JointType.Spine || jointType == JointType.ShoulderCenter || jointType == JointType.ShoulderRight || jointType == JointType.Head;
        }

        /// <summary>
        /// Maps a SkeletonPoint to lie within our render space and converts to Point
        /// </summary>
        /// <param name="skelpoint">point to map</param>
        /// <returns>mapped point</returns>
        private Point SkeletonPointToScreen(SkeletonPoint skelpoint)
        {
            // Convert point to depth space.  
            // We are not using depth directly, but we do want the points in our 640x480 output resolution.
            DepthImagePoint depthPoint = this.sensor.CoordinateMapper.MapSkeletonPointToDepthPoint(skelpoint, DepthImageFormat.Resolution640x480Fps30);
            return new Point(depthPoint.X, depthPoint.Y);
        }

        /// <summary>
        /// Draws a bone line between two joints
        /// </summary>
        /// <param name="skeleton">skeleton to draw bones from</param>
        /// <param name="drawingContext">drawing context to draw to</param>
        /// <param name="jointType0">joint to start drawing from</param>
        /// <param name="jointType1">joint to end drawing at</param>
        private void DrawBone(Skeleton skeleton, DrawingContext drawingContext, JointType jointType0, JointType jointType1)
        {
            Joint joint0 = skeleton.Joints[jointType0];
            Joint joint1 = skeleton.Joints[jointType1];

            // If we can't find either of these joints, exit
            if (joint0.TrackingState == JointTrackingState.NotTracked ||
                joint1.TrackingState == JointTrackingState.NotTracked)
            {
                return;
            }

            // Don't draw if both points are inferred
            if (joint0.TrackingState == JointTrackingState.Inferred &&
                joint1.TrackingState == JointTrackingState.Inferred)
            {
                return;
            }

            // We assume all drawn bones are inferred unless BOTH joints are tracked
            Pen drawPen = this.inferredBonePen;
            if (joint0.TrackingState == JointTrackingState.Tracked && joint1.TrackingState == JointTrackingState.Tracked)
            {
                drawPen = this.trackedBonePen;
            }

            drawingContext.DrawLine(drawPen, this.SkeletonPointToScreen(joint0.Position), this.SkeletonPointToScreen(joint1.Position));
        }

        /// <summary>
        /// Event handler for Kinect sensor's DepthFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void SensorDepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            {
                if (depthFrame != null)
                {
                    try
             {
                _interactionStream.ProcessDepth(depthFrame.GetRawPixelData(), depthFrame.Timestamp);
            }
           catch (InvalidOperationException)
            {
               // DepthFrame functions may throw when the sensor gets
                // into a bad state.  Ignore the frame in that case.
           }

                    // Copy the pixel data from the image to a temporary array
                    depthFrame.CopyDepthImagePixelDataTo(this.depthPixels);

                    // Get the min and max reliable depth for the current frame
                    int minDepth = depthFrame.MinDepth;
                    int maxDepth = depthFrame.MaxDepth;

                    // Convert the depth to RGB
                    int colorPixelIndex = 0;
                    for (int i = 0; i < this.depthPixels.Length; ++i)
                    {
                        // Get the depth for this pixel
                        short depth = depthPixels[i].Depth;

                        // To convert to a byte, we're discarding the most-significant
                        // rather than least-significant bits.
                        // We're preserving detail, although the intensity will "wrap."
                        // Values outside the reliable depth range are mapped to 0 (black).

                        // Note: Using conditionals in this loop could degrade performance.
                        // Consider using a lookup table instead when writing production code.
                        // See the KinectDepthViewer class used by the KinectExplorer sample
                        // for a lookup table example.
                        byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                        // Write out blue byte
                        this.colorPixels[colorPixelIndex++] = intensity;

                        // Write out green byte
                        this.colorPixels[colorPixelIndex++] = intensity;

                        // Write out red byte                        
                        this.colorPixels[colorPixelIndex++] = intensity;
                                                
                        // We're outputting BGR, the last byte in the 32 bits is unused so skip it
                        // If we were outputting BGRA, we would write alpha here.
                        ++colorPixelIndex;
                    }

                    // Write the pixel data into our bitmap
                    this.colorBitmap.WritePixels(
                        new Int32Rect(0, 0, this.colorBitmap.PixelWidth, this.colorBitmap.PixelHeight),
                        this.colorPixels,
                        this.colorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }
        }
        #endregion
       
        
        /// <summary>
        /// Handles the checking or unchecking of the near mode combo box
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void CheckBoxNearModeChanged(object sender, RoutedEventArgs e)
        {
            if (this.sensor != null)
            {
                // will not function on non-Kinect for Windows devices
                try
                {
                    if (this.checkBoxNearMode.IsChecked.GetValueOrDefault())
                    {
                        this.sensor.DepthStream.Range = DepthRange.Near;
                    }
                    else
                    {
                        this.sensor.DepthStream.Range = DepthRange.Default;
                    }
                }
                catch (InvalidOperationException)
                {
                }
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (null != this.sensor)
            {
                this.sensor.AudioSource.Stop();
                this.sensor.Stop();
            }
            if (null != this.speechEngine)
            {
                this.speechEngine.SpeechRecognized -= SpeechRecognized;
                this.speechEngine.RecognizeAsyncStop();
            }
            readMessage();
            sendMessage("cmd 99\n");//exit 
            try
            {
                s.Close();
            }
            catch (Exception) { }
            myList.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Net.IPAddress ipAd = IPAddress.Parse("138.246.13.102");

                myList = new TcpListener(ipAd, 8001);


                myList.Start();


                s = myList.AcceptSocket();
                Button.Visibility = Visibility.Hidden;
            }
            catch (Exception) { //MessageBox.Show("Connection failed");
            };
        }


    }
}