   M S B A M L   `   `   `  ����  WebStore    OnlineStoreApp.CartWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$
  Cart��5   	   $	��800��6   $
��1200��6#    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � .+�5   
   �� .+�5      �� -   5         ��Name$ CartItemsControl��5      ��5	      �� 5
      .R�5      ��  �� Orientation$ 
Horizontal=�6%   $��5q�6>   .+�5      �� $��0,0,10,0q�6>   ��6(   �� *name+6>   �� 5      $��0,0,10,0q�6B   ��6(   �� *
quantity+6B   �� 5      $��0,0,10,0q�6O   ��6(   �� *price+ �� StringFormat$ C��6O   �� 5      -   5        $��Remove��5   %   
 � Tag 6S   �� 5      5      5      �� 5      $ 
Horizontal=�6   $��Right=�62   $��0,10,0,0q�6N   .+�5      �� $��Total Price:��6   $
��Bold"�60   $��0,0,10,0q�6B   �� 5      -   5         ��Name$ TotalPriceText��5      $
��Bold"�64   5      �� 5      -   5        $��Checkout��5      $��Right=�6?   5      5      5      