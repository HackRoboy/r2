   M S B A M L   `   `   `  ����   DepthBasics-WPF5    /Microsoft.Samples.Kinect.DepthBasics.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     \ XMicrosoft.Kinect.Toolkit.Controls, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullS OMicrosoft.Kinect.Toolkit, Version=1.8.0.0, Culture=neutral, PublicKeyToken=null2k(http://schemas.microsoft.com/kinect/2013   <d2http://schemas.microsoft.com/expression/blend/2008   Fmc;http://schemas.openxmlformats.org/markup-compatibility/2006   -     � Title$  Kinect Controller��$	��735��$	��770�� � 	Resources    MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  SnapshotButton �� 
TargetType �� Orientation ��Name	 �� TextAlignment
 ��Name �� Triggers P� Property P� Value �� 
TargetName  SquareCheckBox �Name��%�  &	        &	      &	 *     '�� ?     &	 t     7  7  &	 e    7  7  �� $ 	#ff6e6e6e���� $ 	#ff52318f���� $ 	#ff00BCF2���� " ��.���  �@J�SnapsToDevicePixels . �� " ��.���  � �� �� " ��.R�� .+��� $ 
Horizontal=�
������ .+��� $ SnapText��#��z��$
	 Left=�$��Center=�8��  $��15(�� $��9,0,0,0q�.+��� $
 
SnapNormal��$��Images\SnapNormal.png��$
��None=�$��Center=��� $
 	SnapHover��$��Images\SnapHover.png��$
��None=�$��Center=�$y�	Collapsed=� P�  �@J�IsMouseOver . . ��� $ 
SnapNormal�� � y�$ 	Collapsed=��� $ 	SnapHover�� � y�$ Visible=��� 8  $ SnapText�� � ���� " ��.���  � �� �� " ��.R�� .+��� $ 
Horizontal=�
������ .+�� $ SquareCheckBoxChecked��.+��� $
 CheckedNormal��$��Images\CheckedNormal.png��$
��None=�$��Center=��� $
 CheckedHover��$��Images\CheckedHover.png��$
��None=�$��Center=�$y�	Collapsed=�� $ SquareCheckBoxUnchecked��$y�	Collapsed=�.+��� $
 UncheckedNormal��$ ��Images\UncheckedNormal.png��$
��None=�$��Center=��� $
 UncheckedHover��$��Images\UncheckedHover.png��$
��None=�$��Center=�$y�	Collapsed=��� $ SquareCheckBoxText��#��z��$
	 Left=�$��Center=�8��  $��15(�$��9,0,0,0q� P�  �@d�	IsChecked$ falseY�. ��� $ SquareCheckBoxChecked�� � y�$ 	Collapsed=��� $ SquareCheckBoxUnchecked�� � y�$ Visible=�P�  �@J�IsMouseOver . . ��� $ CheckedNormal�� � y�$ 	Collapsed=��� $ UncheckedNormal�� � y�$ 	Collapsed=��� $ CheckedHover�� � y�$ Visible=��� $ UncheckedHover�� � y�$ Visible=��� 8  $ SquareCheckBoxText�� � ��.��� -   $ 
layoutGrid��$��	10 0 10 0q� � ColumnDefinitions �� $
��249* ��� $
��122* � � RowDefinitions �� $
��Auto ��� $
��105* ��� $	��11* ��� $
��164* ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$ X��$�� ��$��37��$	��Top=�$��481,10,180,0q� J� RenderTransformOrigin$ -1.102,0.541&�$��2��]� $��0��$��0 0 0 20q�$��2��.+��� $
��Left=�$��Images\Logo.png��$
��Fill=�$��32��$��81��$��0 10 0 5q��� $��1��$��Images\Status.png��$
��None=�$��Center=�$��0 0 80 5q�$ -0.697,0.518&�(� $��1�� (� Stretch$ Uniform=�$��Center=�$��2��$��3��.���� -   $
 Image��$	��640��$	��480���� -    ��Name$ checkBoxNearMode��$��4��#��[ $��	Near Mode��$
��Auto��$
��Left=�$��Center=�$��0,23,0,5q��� -    ��Name$ 	statusBar��$��5��$��Stretch=�$��Bottom=�
�������#��[  $��2��.@��� $��0 0 0 10q�.���� -   $ statusBarText��$��-1 0 0 0q��� $� ��(� $��1��$ Uniform=�$��Center=�$��2��$��3��.���� -   $
 ImageSK��$	��640��$	��480���� -	   $ Y��$�� ��$��37��$	��Top=�$��	0,10,95,0q�$��1��$ -1.102,0.541&�$��Right=�$��80���� -
   $ Z��$�� ��$��37��$	��Top=�$��154,10,10,0q�$��1��$ -1.102,0.541&��� -   $ Event��$�� ��$��37��$	��Top=�$��152,10,227,0q�$ -1.102,0.541&��� -   $ X_Copy��$�� ��$��37��$	��Top=�$��
47,1,138,0q�$��1��$ -1.102,0.541&�$��4���� -   $ Y_Copy��$�� ��$��37��$	��Top=�$��
111,1,74,0q�$��1��$ -1.102,0.541&�$��4���� -   $ Z_Copy��$�� ��$��37��$	��Top=�$��
175,1,10,0q�$��1��$ -1.102,0.541&�$��4���� $4��.Voice Commands: MOVE 	     (Down, Left, Right)��$
��Left=�$��
248,14,0,0q�$��4��$	��Top=�$ -1.03,0.408&����	#FF533290$��20(�$��2��$	��494���� -   $ Orientation��$��Orientation:  Down��$��37��$	��Top=�$��47,322,60,0q�$ -1.102,0.541&�$��3��$��14(�$��1��$��2���� -    ��Name$ Button��$��Connect��$��62,146,60,144q�$��1��$��2��$��3��$��72(�