   M S B A M L   `   `   `  ����C  ?ExDesign, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:ExDesign.Pages.InputsExDesign.Pages.Inputs  5      /clr-namespace:ExDesign.DatasExDesign.Datas  5      *    $ExDesign.Pages.Inputs.SoilMethodPage   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:ExDesign.Pages.Inputs     5      .datasclr-namespace:ExDesign.Datas     5      -   5          N� Title$  SoilMethodPage��5
      .��5      � .+�5   
   �  � RowDefinitions 5      �� 5      �� 5      $	��300 �6    .+�5      � $��0��6    � ColumnDefinitions 5      �� 5      $��* �6'   �� 5      $
��1.5* �6'   .+�5       � -   5          �Name$ view3d_main��5      $��0��6-     � NavigationUIVisibility$ Hidden=�6=    � 5      -   5        $ sideview_main��5      $��1��6/   $ Hidden=�6?   5      �� 5      $��1��6   
�������.+�5      �  5      �� 5      $	��250 �6+   �� 5      $��* �6+   .+�5"      �� $��0��6!   .+�5#      �� -   5         ��Name$ soilModelGroupBox��5#   $   $��5q�6?      	SoilModel#���   6J   .��5$      �  5%   "   �� 5&   &   �� 5'   &   .+�5*   "   ]� $��0��6,   $
��Left=�69   $	��Top=�6T   $��3q�6l   .+�5+   &   �� -   5         ��Name$ soilModelCombobox��5+   /   $	��200��6J   .@�5,   *   �� -   5         ��Name$
 SMC1��5,   7   .��6E   Schmitt Model6T   �� 5-   *   -   5        $
 SMC2��5-   7   .��6E   Chadeisson Model6W   �� 5.   *   -   5        $
 SMC3��5.   7   .��6E   Vesic Model6R   5/   '   50   #   ]� 51   "   -	   5         ]�Name$ SchmittInfo��51   9   $��1��6,   $y�Hidden=�6N   $
��Left=�6b   $	��Top=�6}   .+�53   &   ��   SchmittInfo#���  62   $��WordEllipsis=�6W   	 �� LineStackingStrategy$	 	MaxHeight=�6s   $
��Wrap=�6�   54   #   ]� 55   "   -
   5        $ ChadeissonInfo��55   9   $��1��6,   $y�Hidden=�6Q   $
��Left=�6e   $	��Top=�6�   .+�57   &   ��   ChadeissonInfo#���  62   $��WordEllipsis=�6Z   $	 	MaxHeight=�6v   $
��Wrap=�6�   58   #   ]� 59   "   -   5        $ 	VesicInfo��59   9   $��1��6,   $y�Hidden=�6L   $
��Left=�6`   $	��Top=�6{   .+�5;   &   ��   	VesicInfo#���  62   $��WordEllipsis=�6U   $	 	MaxHeight=�6q   $
��Wrap=�6�   5<   #   5=      5>      5?      �� 5@      $��1��6!   .+�5A      �� $��5q�6%     
SoilLayers#���  60   .��5B      �� .+�5C   "   ]� $
��Left=�6,   .+�5D   &   �� -   5        
 ��Name$
 addsoillayer_bttn��5D   -     AddSoilLayer#���  6H   $	��150��6q   $��10q�6}   6�   �� 5E   &   -   5        $
 soilTypeLibrary��5E   -     SoilRockLibrary#���  6F   $	��150��6r   $��10q�6~   6�   5F   #   �� 5G   "   -   5         ��Name$ headerScrollBar��5G   /   $��Hidden=�6H   $��Hidden=�6m   $��False��6�   .��5H   &   ]� $
��Left=�60   $��2q�6K   $��27��6V   .+�5I   *   �� ${�False��62   $��27��6D   $�� ��6O   $��Center=�6W   $��Center=�6y   $
��Bold"�6�   6�   �� 5J   *   ${�False��62   $��30��6D   $��No��6O   $��Center=�6Y   $��Center=�6{   $
��Bold"�6�   6�   �� 5K   *   ${�False��62   $��80��6D    
 Height#���  6O   $��Center=�6o   $��Center=�6�   $
��Bold"�6�   6�   �� 5L   *   ${�False��62   $��55��6D     Texture#���  6O   $��Center=�6p   $��Center=�6�   $
��Bold"�6�   6�   �� 5M   *   ${�False��62   $	��300��6D    	 AssignedSoilName#��� 	 6P   $��Center=�6z   $��Center=�6�   $
��Bold"�6�   6�   �� 5N   *   ${�False��62   $��80��6D   $��γ��6O   $��Center=�6X   $��Center=�6z   $
��Bold"�6�   6�   �� 5O   *   ${�False��62   $��80��6D   $	��γd��6O   $��Center=�6Y   $��Center=�6{   $
��Bold"�6�   6�   �� 5P   *   ${�False��62   $��80��6D   $��Φ��6O   $��Center=�6X   $��Center=�6z   $
��Bold"�6�   6�   �� 5Q   *   ${�False��62   $��80��6D   $��c'��6O   $��Center=�6Y   $��Center=�6{   $
��Bold"�6�   6�   �� 5R   *   ${�False��62   $��80��6D   $��Cu��6O   $��Center=�6Y   $��Center=�6{   $
��Bold"�6�   6�   �� 5S   *   ${�False��62   $��80��6D   $	��δd��6O   $��Center=�6Y   $��Center=�6{   $
��Bold"�6�   6�   �� 5T   *   ${�False��62   $��80��6D   $��ν��6O   $��Center=�6X   $��Center=�6z   $
��Bold"�6�   6�   �� 5U   *   ${�False��62   $��80��6D    
 	SoilState#��� 
 6O   $��Center=�6r   $��Center=�6�   $
��Bold"�6�   6�   �� 5V   *   ${�False��62   $��80��6D   $	��OCR��6O   $��Center=�6Z   $��Center=�6|   $
��Bold"�6�   6�   �� 5W   *   ${�False��62   $��80��6D   $��K0��6O   $��Center=�6Y   $��Center=�6{   $
��Bold"�6�   6�   �� 5X   *   -   5         ��Name$ selected_parameter��5X   2   ${�False��6N   $��80��6`   $��K0��6k   $��Center=�6u   $��Center=�6�   $
��Bold"�6�   6�   ]� 5Y   *   $��30��64   6A   5Z   '   5[   #   �� 5]   "   -   5        $ LayerGridScrollBar��5]   /   $
��Auto=�6K   $
��Auto=�6n   $	��200��6�   .��5^   &   �� -   5         ��Name$ soilLayerGroupbox��5^   1   6P   5`   #   5a      5c      5d      5f      5g      5h      5i      5j      