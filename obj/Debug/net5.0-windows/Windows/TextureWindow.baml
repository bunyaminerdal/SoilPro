   M S B A M L   `   `   `  ����C  ?ExDesign, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:ExDesign.WindowsExDesign.Windows  5   	   $    ExDesign.Windows.TextureWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:ExDesign.Windows     5   	   -   5          � Title$  TextureWindow��5   	   $	��450��6   $	��800��6,    � WindowStyle$ 
ToolWindow=�68    � 
ResizeMode$ NoResize=�6Q   .��5	      �  � RowDefinitions 5
   
   �� 5      $	��120 �6   �� 5      �� 5      $��30 �6   .+�5   
   ]� $��2��6   $��Right=�6!   .+�5      �� -   5         ��Name$ select_button��5       
  Select#���   6-   $
��Left=�6P   $	��Top=�6l   $��50��6�   $��5q�6�   6�   �� 5      -   5        $ cancel_button��5       
 Cancel#���  6-   $
��Left=�6P   $	��Top=�6l   $��50��6�   $��5q�6�   6�   5      �� 5   
   -   5         ��Name$ TexturePanel��5      $��1��6+   6:   5      5      