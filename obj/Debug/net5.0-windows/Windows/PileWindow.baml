   M S B A M L   `   `   `  ����C  ?ExDesign, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ExDesign.WindowsExDesign.Windows  5   	   !    ExDesign.Windows.PileWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:ExDesign.Windows     5   	   -   5           pileDiameter  � Title#  �   5   	   $	��270��60   $	��370��6=    � WindowStyle$ 
ToolWindow=�6I    � 
ResizeMode$ NoResize=�6y   .��5	      � .+�5
   
   �� -   5         ��Name$ PileList��5
      $	��100��6$   $	��165��60   $	��Top=�6=   $
��Left=�6U   $��	10,35,0,0q�6p   6�   �� 5   
   -   5         ��Name$ deletepile_button��5       
 delete#���  6,   $
��Left=�6O   $��
10,205,0,0q�6j   $	��Top=�6~   $	��100��6�   6�   �� 5   
     pileName#���  6   $
��Left=�65   $��
138,40,0,0q�6P   $	��Top=�6d   �� 5   
   $��:��6   $
��Left=�6   $��
230,40,0,0q�67   $	��Top=�6K   �� 5   
   -   5         ��Name$ pilenametextbox��5       �� 	MaxLength$ 10��6+   $
��Left=�6:   $��
240,45,0,0q�6U   $	��Top=�6j   $��78��6�   6�   �� 5   
   #���   6   $
��Left=�69   $��
138,66,0,0q�6T   $	��Top=�6h   �� 5   
   $
��(t):��6   $
��Left=�6   $��
219,66,0,0q�6:   $	��Top=�6N   �� 5   
   -   5        $ pilediametertextbox��5      $
��Left=�6/   $��
240,71,0,0q�6J    �� TextWrapping$
 Wrap=�6^   $	��Top=�6s   $��78��6�   6�   �� 5   
   -   5         ��Name$ pilediameterunit��5      $��m��6*   $
��Left=�66   $��
323,66,0,0q�6Q   $	��Top=�6e   �� 5   
   -   5        $ cancel_button��5       
 Cancel#���  6(   $
��Left=�6K   $��293,205,0,0q�6f   $	��Top=�6{   $��50��6�   6�   �� 5   
   -   5        $ save_button��5        Save#���  6&   $
��Left=�6G   $��238,205,0,0q�6b   $	��Top=�6w   $��50��6�   6�   �� 5   
   -	   5        $ addnewpile_button��5       
 addnew#���  6,   $
��Left=�6O   $��	10,10,0,0q�6j   $	��Top=�6}   $	��100��6�   6�   �� 5   
   -
   5        $ save_close_button��5        SaveAndClose#���  6,   $
��Left=�6U   $��143,205,0,0q�6p   $	��Top=�6�   $��90��6�   6�   5      5      