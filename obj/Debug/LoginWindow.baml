   M S B A M L   `   `   `  ����  WebStore     OnlineStoreApp.LoginWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Login��5   	    � WindowStartupLocation$ CenterScreen=�5   	   ���#F9F9F9 � WindowState$ 	Maximized=�5   
    � 
ResizeMode$ NoResize=�6#   .��5      � $��20q�6    � RowDefinitions 5	   
   �� 5
      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   $��0,0,0,20q�6X   .+�5      �� $��Admin Login:��6   $
��Bold"�6,   $��14(�6>   ���#333$��Center=�6^   $��0,0,10,0q�6y   �� 5      -   5         ��Name$ AdminModeCheckBox��5      $��Enable��62   6g   5      �� 5   
   $��1��6   
������� �� CornerRadius$ 10��61    �� Padding$ 20q�6C   ���#DDD$��1q�6c   ._�5      � .+�5      �� -   5        	 ��Name$	 UserLoginPanel��5      $��Center=�65   $��Center=�6P   .+�5      �� $��
User Login��6    $
��Bold"�62   $��18(�6D   ���#333$��0,0,0,10q�6d   �� 5      -   5        
 ��Name$
 UsernameTextBox��5      $	��250��68   $��0,0,0,10q�6D   ���#F3F3F3���#DDD$��10q�6~   G� 5      -   5         G�Name$ PasswordTextBox��5   "   $	��250��6<   $��0,0,0,20q�6H   ���#F3F3F3���#DDD$��10q�6�   �� 5      -   5        $��Login as User��5      $	��200��65   ���#333
����������#3336y   5      �� 5!      -   5        $	 AdminLoginPanel��5!      $��Center=�66   $��Center=�6Q   $y�	Collapsed=�6n   .+�5"      �� $��Admin Login��6    $
��Bold"�63   $��18(�6E   ���#333$��0,0,0,10q�6e   �� 5#      -   5        $
 AdminUsernameTextBox��5#      $	��250��6=   $��0,0,0,10q�6I   ���#F3F3F3���#DDD$��10q�6�   G� 5$      -   5        $ AdminPasswordTextBox��5$   "   $	��250��6A   $��0,0,0,20q�6M   ���#F3F3F3���#DDD$��10q�6�   �� 5%      -	   5        $��Login as Admin��5%      $	��200��66   ���#333
����������#3336z   5&      5'      5(      5*      5+      