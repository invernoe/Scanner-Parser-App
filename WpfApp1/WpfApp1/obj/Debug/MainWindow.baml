   M S B A M L   `   `   `  ����O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  5   	   B >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1 5   	      WpfApp1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  
MainWindow��5	   	   $
��Auto��6   $
��Auto��6*      WindowBackgroundBrush#���   5
   	    � 	Resources 5       � Offset � Color��%|   &	        �� 5   
   .��5      � $ 1Q�6   $ Silver��6&   � 5      $ 0Q�6   $
 Gray��6&   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��2* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��1* �6   .+�5   
   �� -   5        $��txtInput��5      $��Stretch=�5      $��Stretch=�5      $��8q�5       �� AcceptsReturn .  �� 
AcceptsTab .  �� TextWrapping$
 Wrap=�5!      " �� VerticalScrollBarVisibility$ Visible=�5"      $
��Auto��5#      6"   �� 5%   
   -   5        $��	runButton��5%      $
��Thin"�5&      $��Arial)�5'      $��24(�5(      $��40��5)      $��80��5*      $��0,0,8,24q�5+      $��Right=�5,      $��Center=�5-      $��1��5.      .��5/   )   Run51      �� 53   
   $��24(�6   $��8,0,0,0q�54      $
��Thin"�55      $��Arial)�56      $��2��57      	Output:6*   �� 59   
   -   5        $��	txtOutput��59      $��8q�5:      $��3��5;       . $
 Wrap=�5=      $ Visible=�5>      	 �� 
IsReadOnly	 . 5?      5@      5B      