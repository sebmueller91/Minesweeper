   M S B A M L   `   `   `  ����F  BMinesweeper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:MinesweeperMinesweeper  5   	   #    Minesweeper.MinesweeperWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Minesweeper     5   	     � Title$  Minesweeper��5   	    � SizeToContent$ WidthAndHeight=�6   .��5	      � -   5        $��	MAIN_GRID��5	       � RowDefinitions 5
   
   �� 5      $��30 �6   �� 5       � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $	��50* �6   �� 5      $��1* �6   .+�5   
   �� $��1��6    �� Orientation$ 
Horizontal=�6%   $
��Left=�6>   .+�5      �� $��Mines: ��6   $��15(�6'   �� 5      -   5        $��NUMBER_MINES��5   &   $��15(�6   ��6:   ��  �� ElementName$ NUMBER_MINES�� �� Path$ Value� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5      �� 5   
   $��1��6   $ 
Horizontal=�6%   $��Right=�6>   .+�5      �� $��Time: ��6   $��15(�6&   �� 5      -   5        $
��TIME��5   &   $��15(�6   ��62   �� $
 TIME��$ Value�$ TwoWay=�$ PropertyChanged=�5      A� 5    
   -   5        $��GRID_VAR��5    *   $��3��6   $��1��6:   5"      5#      5%      