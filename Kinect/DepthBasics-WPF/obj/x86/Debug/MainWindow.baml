   M S B A M L   `   `   `  ����  DepthBasics-WPF5    /Microsoft.Samples.Kinect.DepthBasics.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   \ XMicrosoft.Kinect.Toolkit.Controls, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullS OMicrosoft.Kinect.Toolkit, Version=1.8.0.0, Culture=neutral, PublicKeyToken=null2k(http://schemas.microsoft.com/kinect/2013   5   	   <d2http://schemas.microsoft.com/expression/blend/2008   5   	   Fmc;http://schemas.openxmlformats.org/markup-compatibility/2006   6F   -   5          � Title$  Kinect Controller��5   	   $	��735��6#   $	��770��60    � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  SnapshotButton �� 
TargetType �� Orientation ��Name	 �� TextAlignment
 ��Name �� Triggers P� Property P� Value �� 
TargetName  SquareCheckBox �Name��%w
  &	        &	 #     &	 F     '�� i     &	 �     7  7  &	 �    7  7  �� 5	   
   $ 	#ff6e6e6e��62   �� 5
   
   $ 	#ff52318f��64   �� 5   
   $ 	#ff00BCF2��62   �� 5   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels . 64   5      �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $ 
Horizontal=�6)   
������ .+�5   "   �� $ SnapText��6,   #��z��6>   $
	 Left=�6_   $��Center=�6u   8��  $��15(�6�   � 5   "   $��9,0,0,0q�6'   .+�5   &   �� $
 
SnapNormal��6,   $��Images\SnapNormal.png��6@   $
��None=�6_   $��Center=�6n   �� 5   &   $
 	SnapHover��6,   $��Images\SnapHover.png��6?   $
��None=�6]   $��Center=�6l   $y�	Collapsed=�6�   5   #   5      5       5      P� 5       �@J�IsMouseOver . . �5   "   �� $ 
SnapNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5   "   $ 	SnapHover��6O    � y�$ Visible=�6?   6O   �� 5    "   8  $ SnapText��6h    � ��5!      5#      5%      5&      �� 5'   
   " ��6   .�5(      ��  � �� 5)      �� 5*      " ��6&   .R�5+      � .+�5,      �� $ 
Horizontal=�6)   
������ .+�5-   "   � $ SquareCheckBoxChecked��6'   .+�5.   &   �� $
 CheckedNormal��6,   $��Images\CheckedNormal.png��6C   $
��None=�6e   $��Center=�6t   �� 5/   &   $
 CheckedHover��6,   $��Images\CheckedHover.png��6B   $
��None=�6c   $��Center=�6r   $y�	Collapsed=�6�   50   #   � 51   "   $ SquareCheckBoxUnchecked��6'   $y�	Collapsed=�6H   .+�52   &   �� $
 UncheckedNormal��6,   $ ��Images\UncheckedNormal.png��6E   $
��None=�6i   $��Center=�6x   �� 53   &   $
 UncheckedHover��6,   $��Images\UncheckedHover.png��6D   $
��None=�6g   $��Center=�6v   $y�	Collapsed=�6�   54   #   �� 55   "   $ SquareCheckBoxText��6,   #��z��6H   $
	 Left=�6i   $��Center=�6   8��  $��15(�6�   $��9,0,0,0q�6�   56      57       58      P� 59       �@d�	IsChecked$ falseY�6;   . �5:   "   �� $ SquareCheckBoxChecked��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5;   "   $ SquareCheckBoxUnchecked��6O    � y�$ Visible=�6?   6O   5<      P� 5=       �@J�IsMouseOver . . �5>   "   �� $ CheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5?   "   $ UncheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5@   "   $ CheckedHover��6O    � y�$ Visible=�6?   6O   �� 5A   "   $ UncheckedHover��6O    � y�$ Visible=�6?   6O   �� 5B   "   8  $ SquareCheckBoxText��6h    � ��5C      5E      5G      5H      5I      .��5L      � -   5        $ 
layoutGrid��5L      $��	10 0 10 0q�6    � ColumnDefinitions 5M   
   �� 5N      $
��249* �6   �� 5O      $
��122* �6    � RowDefinitions 5Q   
   �� 5R      $
��Auto �6   �� 5S      $��* �6   �� 5T      $
��Auto �6   �� 5U      $
��Auto �6   .+�5Y   
   �� -   5         ��Name$ X��5Y      $�� ��6   $��37��6&   $	��Top=�62   $��47,10,138,0q�6J   $��1��6_    J� RenderTransformOrigin$ -1.102,0.541&�6o   ]� 5\   
   $��0��6   $��0 0 0 20q�6!   $��2��63   .+�5]      �� $
��Left=�6   $��Images\Logo.png��6*   $
��Fill=�6C   $��32��6R   $��81��6^   $��0 10 0 5q�6i   �� 5_      $��1��6   $��Images\Status.png��6$   $
��None=�6?   $��Center=�6N   $��0 0 80 5q�6k   $ -0.697,0.518&�6}   5`      (� 5a   
   $��1��6    (� Stretch$ Uniform=�6   $��Center=�61   $��2��6N   .��5b      �� -   5        $
 Image��5b      $	��640��6#   $	��480��6/   5c      �� 5e   
   -   5         ��Name$ checkBoxNearMode��5e   �   $��2��6   #��[ 6    $��	Near Mode��6H   $
��Auto��6\   $
��Left=�6j   $��Center=�6�   $
��0,14q�6�   6�   �� 5f   
   -   5         ��Name$ 	statusBar��5f   ?   $��3��6   $��Stretch=�6!   $��Bottom=�6R   
�������#��[  6�   $��2��6�   .@�5g      �� $��0 0 0 10q�6   .��5h      �� -   5        $ statusBarText��5h      $��-1 0 0 0q�63   �� 6F   $N�HPress 'Screenshot' to save a screenshot to your 'My Pictures' directory.��6J   6�   5i      5j      (� 5k   
   $��1��6   $ Uniform=�6   $��Center=�61   $��2��6N   .��5l      �� -   5        $
 ImageSK��5l      $	��640��6%   $	��480��61   5m      �� 5n   
   -	   5        $ Y��5n      $�� ��6   $��37��6&   $	��Top=�62   $��111,10,74,0q�6J   $��1��6_   $ -1.102,0.541&�6o   �� 5o   
   -
   5        $ Z��5o      $�� ��6   $��37��6&   $	��Top=�62   $��175,10,10,0q�6J   $��1��6_   $ -1.102,0.541&�6o   �� 5p   
   -   5        $ Event��5p      $�� ��6   $��37��6*   $	��Top=�66   $��152,10,227,0q�6N   $ -1.102,0.541&�6d   5q      5s      