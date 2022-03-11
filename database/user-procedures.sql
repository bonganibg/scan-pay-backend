-- Create account
reate procedure manageUserAccount
(
  @UserID varchar(40), -- generated in the API when creating account
  @Name varchar(50),
  @Username varchar(30),
  @Email varchar(40),
  @PhoneNumber varchar(12),
  @Password varchar(50), -- hashed password
  @PassPhrase varchar(100)
)
as begin
  insert into Users values (@UserID, @Name, @Username, @Email,@PhoneNumber, @Password, @PassPhrase);
end

-- Login
create procedure userLogin
(
  @Email varchar(40),
  @Password varchar(50)
)
as begin
  select user_id from Users
  where email = @Email
  and password = @Password
end

-- Update user Information
create procedure updateUserInfo
(
  @UserID varchar(40), -- generated in the API when creating account
  @Name varchar(50),
  @Username varchar(30),
  @Email varchar(40),
  @PhoneNumber varchar(12),
  @Password varchar(50) -- hashed password
)
as begin
  update Users
  set
  full_name =@Name,
  username = @Username,
  email = @Email,
  phone_number = @PhoneNumber,
  password = @Password
  where user_id =@UserID
end
