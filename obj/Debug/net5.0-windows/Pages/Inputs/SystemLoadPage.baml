   M S B A M L   `   `   `  ����C  ?ExDesign, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:ExDesign.Pages.Inputs.ViewsExDesign.Pages.Inputs.Views  5      *    $ExDesign.Pages.Inputs.SystemLoadPage   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:ExDesign.Pages.Inputs.Views     5      -   5          N� Title$  SystemLoadPage��5	      .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $	��300 �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��1.5* �6#   .+�5       � -   5          �Name$ view3d_main��5      $��0��6)     � NavigationUIVisibility$ Hidden=�69    � 5      -   5        $ sideview_main��5      $��1��6+   $ Hidden=�6;   5      �� 5   
   $��1��6   
�������.+�5      �  5      �� 5      $��* �6(   �� 5      $��* �6(   �� 5      $��* �6(   �� 5      $
��1.6* �6(   .+�5!      �� $��0��6   .+�5"      �� $��	0,5,2.5,5q�6      EarthquakeDesign#���   62   .��5#      �� .+�5$      ]� $
��Left=�6(   $	��Top=�6C   $��5q�6[   .+�5%   "   �� -   5         ��Name$ earthquakeCheckbox��5%   +   #���   6G   6�   5&      ]� 5'      $
��True��6(   $	��Top=�6=   $��5q�6U   .+�5(   "   �� $��40��6(   $��SDS :��63   �� 5)   "   -   5         ��Name$ sds_textbox��5)   +   $	��100��6@   $��22��6L   $
��Left=�6X   6�   �� 5*   "   -   5        $ sdscheckbox��5*   +   $��16��6@   $��16��6K   $��5q�6W   $
��Left=�6b   $��Center=�6}   6�   5+      ]� 5,      $
��True��6(   $	��Top=�6=   $��5,5,5,2q�6U   .+�5-   "   �� $��40��6(   $
��Kh :��63   �� 5.   "   -   5        $ 
kh_textbox��5.   +   $	��100��6?   $��22��6K   $
��Left=�6W   6�   5/      ]� 50      $
��True��6(   $	��Top=�6=   $��5,2,5,2q�6U   .+�51   "   �� $��40��6(   $
��Kv :��63   �� 52   "   -   5        $ 
kv_textbox��52   +   $	��100��6?   $��22��6K   $
��Left=�6W   6�   53      54      55      56      �� 57      $��1��6   .+�58      �� $��	0,5,2.5,5q�6     StageConstruction#���  62   .��59      �� .+�5:      ]� $
��Left=�6(   $	��Top=�6C   $��5q�6[   .+�5;   "   �� -	   5        $ stageconstructionCheckbox��5;   +   #���  6N   6�   5<      ]� 5=      $��5q�6*   .+�5>   "   ��   DepthAfterAnchorsStruts#���  6)   5?      ]� 5@      $��5q�6+   .+�5A   "   �� $��50��6(   $��(depth):��63   �� 5B   "   -
   5        $" depthafteranchorstruttextbox��5B   *   $��22��6P   $	��100��6\   6�   �� 5C   "   -   5         ��Name$' !depthafteranchorstruttextbox_unit��5C   (   $��30��6S   $��cm��6^   5D      5E      5F      5H      �� 5I      $��2��6   .+�5J      �� $��	0,5,2.5,5q�6     AnalysMethod#���  62   .��5K      �� .+�5L      ]� $
��Left=�6)   $	��Top=�6D   $��3q�6\   .+�5M   "   �� -   5         ��Name$ analysmethodCombobox��5M   +   $	��150��6J   .@�5N   &   �� -   5        	 ��Name$	 analizmodel1��5N   ^     ClassicLoading#���  63   6^   �� 5O   &   -   5        $	 analizmodel2��5O   `     EquivalentLinear#���  63   6`   �� 5P   &   -   5        $	 analizmodel3��5P   T     FHVA#���  63   6T   5Q   #   5R      ]� 5S      -   5        
 ]�Name$
 classicloadingInfo��5S   )   $y�	Collapsed=�6E   $
��Left=�6\   $	��Top=�6w   .+�5U   "   ��   ClassicLoadingInfo#���  6.   $��WordEllipsis=�6Z    �� LineStackingStrategy$ 	MaxHeight=�6v   $
��Wrap=�6�   5V      ]� 5W      -   5        $
 equivalentLinearInfo��5W   )   $y�	Collapsed=�6G   $
��Left=�6^   $	��Top=�6y   .+�5Y   "   ��   EquivalentLinearInfo#���  6.   $��WordEllipsis=�6\   $ 	MaxHeight=�6x   $
��Wrap=�6�   5Z      ]� 5[      -   5        $
 FHVAinfo��5[   )   $y�	Collapsed=�6;   $
��Left=�6R   $	��Top=�6m   .+�5]   "   ��  	 FHVAinfo#��� 	 6.   $��WordEllipsis=�6P   $ 	MaxHeight=�6l   $
��Wrap=�6�   5^      5_      5`      5b      �� 5c      $��3��6   .+�5d      ��  
 ActivePassiveCoefficients#��� 
 6   .��5e      �� .+�5f      � $��10q�6#    5g   "   �� 5h   &   $��* �67   �� 5i   &   $
��1.6* �67   �� 5j   &   $
��1.6* �67    5l   "   �� 5m   &   $��22 �64   �� 5n   &   $��22 �64   �� 5o   &   $��22 �64   .+�5q   "   �� $��0��6,   $��0��69   6K   �� 5r   "   $��0��6,   $��1��69     Drained#���  6I   $��Center=�6j   6�   �� 5s   "   $��0��6,   $��2��69     	UnDrained#���  6I   $��Center=�6l   6�   �� 5t   "   $��1��6,   $��0��69    
 Active#���  6I   $��Center=�6i   6�   �� 5u   "   -   5        $ ActiveDrained_combobox��5u   +   ${�False��6K   $��1��6]   $��1��6j   .@�5v   &   ��   TBDY#���  63   �� 5w   &     MazindraniTheory#���  63   �� 5x   &     TheColoumbTheory#���  63   �� 5y   &     RankineTheory#���  63   5z   #   �� 5{   "   -   5        $ ActiveUndrained_combobox��5{   +   ${�False��6M   $��1��6_   $��2��6l   .@�5|   &   �� #���  63   �� 5}   &   #���  63   �� 5~   &   #���  63   �� 5   &   #���  63   �� 5�   &     TotalStress#���  63   5�   #   �� 5�   "   $��2��6,   $��0��69     Passive#���  6I   $��Center=�6j   6�   �� 5�   "   -   5        $ PassiveDrained_combobox��5�   +   ${�False��6L   $��2��6^   $��1��6k   .@�5�   &   �� #���  63   �� 5�   &   #���  63   �� 5�   &   #���  63   �� 5�   &   #���  63   5�   #   �� 5�   "   -   5        $ PassiveUndrained_combobox��5�   +   ${�False��6N   $��2��6`   $��2��6m   .@�5�   &   �� #���  63   �� 5�   &   #���  63   �� 5�   &   #���  63   �� 5�   &   #���  63   �� 5�   &   #���  63   5�   #   5�      �� 5�      -   5        $ changeTheoryCheckbox��5�   '   $��10q�6E     ChangeTheory#���  6Q   6�   5�      5�      5�      5�      5�      5�      5�      