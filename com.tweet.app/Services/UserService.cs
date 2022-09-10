// using System.Globalization;
// using System.Text.RegularExpressions;
// using com.tweet.app.DAO;
// using com.tweet.app.Services;
// using com.tweet.app.Services.Exception;
// using TweetApp.Model.Model;
// using UserDetails = com.tweet.app.Model.UserDetails;
//
// namespace com.tweet.app.Services;
//
// public class UserService : IUserService
// {
//     private readonly ITweetRepository _tweetRepository;
//     private readonly IUserRepository _userRepository;
//     private string _currentUser = null!;
//     private bool _isLoggedIn;
//
//     public UserService(IUserRepository userRepository, ITweetRepository tweetRepository)
//     {
//         _userRepository = userRepository;
//         _tweetRepository = tweetRepository;
//     }
//
//     public void Welcome()
//     {
//         Console.WriteLine("\t __________________________________________");
//
//         if (_isLoggedIn)
//             Console.WriteLine($" Hi {_currentUser}");
//         Console.WriteLine("\t\t Welcome to Tweet App \t\t");
//         Console.WriteLine("\t __________________________________________");
//     }
//
//     public bool UserRegistration()
//     {
//         Welcome();
//         Console.WriteLine("--- \t Enter Your Details \t ---");
//         var userRegistration = new UserDetails();
//         var userResponse = string.Empty;
//         while (userResponse == string.Empty)
//         {
//             Console.Write($"{"First Name : ",-10}");
//             userResponse = Console.ReadLine();
//             if (userResponse == string.Empty)
//                 Console.WriteLine("---  First Name is Mandatory--- ");
//             else
//                 userRegistration.FirstName = userResponse;
//         }
//
//         Console.Write($"{"Last Name : ",-10}");
//         userResponse = Console.ReadLine();
//         userRegistration.LastName = userResponse;
//
//         userResponse = string.Empty;
//         while (userResponse == string.Empty)
//         {
//             var gender = Gender(userResponse);
//             userResponse = gender;
//             userRegistration.Gender = gender;
//         }
//
//         userResponse = string.Empty;
//         while (userResponse == string.Empty)
//         {
//             Console.Write("{0,-10}", "DOB(dd-MM-yyyy) : ");
//             userResponse = Console.ReadLine();
//             var dateTime = new DateTime();
//
//             var validDate = DateTime.TryParseExact(userResponse, "dd-MM-yyyy", DateTimeFormatInfo.InvariantInfo,
//                 DateTimeStyles.None, out dateTime);
//             if (validDate)
//             {
//                 userRegistration.DOB = dateTime;
//             }
//             else
//             {
//                 userResponse = string.Empty;
//                 Console.WriteLine(" --- Invalid Date format --- ");
//             }
//         }
//
//         userResponse = string.Empty;
//         while (userResponse == string.Empty)
//         {
//             var data2 = string.Empty;
//             Console.Write("{0,-10}", "Email Id : ");
//             userResponse = Console.ReadLine();
//             if (userResponse == string.Empty)
//             {
//                 Console.WriteLine("---  EmailID is Mandatory --- ");
//             }
//             else
//             {
//                 try
//                 {
//                     ValidateEmail(userResponse);
//                     data2 = userResponse;
//
//                     var duplicateFlag = _userRepository.DuplicateCheck(userResponse);
//
//                     if (duplicateFlag)
//                     {
//                         Console.WriteLine($"{data2} email already exists");
//
//                         data2 = ReEnterEmail(userResponse);
//                     }
//                 }
//
//                 catch (InvalidEmailException exception)
//                 {
//                     Console.WriteLine(exception.Message);
//                     data2 = ReEnterEmail(userResponse);
//                 }
//
//                 userResponse = data2;
//             }
//
//             userRegistration.EmailId = userResponse;
//         }
//
//         userResponse = string.Empty;
//         while (userResponse == string.Empty)
//         {
//             Console.Write("{0,-10}", "Password : ");
//             userResponse = Console.ReadLine();
//             if (userResponse == string.Empty)
//                 Console.WriteLine("--- Password is Mandatory --- ");
//             else
//                 userRegistration.Password = userResponse;
//
//             userResponse = string.Empty;
//             Console.Write("{0,-10}", "Confirm Password : ");
//             userResponse = Console.ReadLine();
//             if (userResponse == string.Empty)
//             {
//                 Console.WriteLine("--- Please confirm your password ---");
//             }
//             else
//             {
//                 if (userRegistration.Password == userResponse)
//                 {
//                     Console.WriteLine("--- Password Matches ---");
//                 }
//                 else
//                 {
//                     Console.WriteLine("--- Password Mismatch -- ");
//                     userResponse = string.Empty;
//                 }
//             }
//         }
//
//         _currentUser = userRegistration.EmailId;
//         _isLoggedIn = true;
//         return _userRepository.UserRegistration(userRegistration);
//     }
//
//     public void IntroAllUsers()
//     {
//         Welcome();
//         Console.WriteLine($"{"1.Create Account  ",-25}|{"2. Login",20}");
//         Console.WriteLine($"{"3.Forget Password  ",-25}|{"4. Reset Password",25}");
//         Console.WriteLine("Enter -1 to exit");
//         Console.Write("\nProvide your response (1/2/3/4) : ");
//
//         var response = Console.ReadLine();
//
//         switch (response)
//         {
//             case "1" when UserRegistration():
//                 Console.WriteLine("\n \t\t Registration Successful! \t\t ");
//                 UserMenu();
//                 break;
//             case "1":
//                 Console.WriteLine("\n --- Registration Failed. Please Try again --- ");
//                 break;
//             case "2":
//             {
//                 while (!UserLogin()) Console.WriteLine("--- Login Failed. Check Credentials--- ");
//                 Console.WriteLine("\n---  Login Successful --- ");
//                 UserMenu();
//                 break;
//             }
//             case "3":
//                 ForgetPassword();
//                 break;
//             case "4":
//                 ResetPassword();
//                 break;
//             case "-1":
//                 Environment.Exit(0);
//                 break;
//             default:
//                 Console.WriteLine("\n--- Invalid Response--- ");
//                 break;
//         }
//     }
//
//
//     public void UserMenu()
//     {
//         Welcome();
//         Console.WriteLine("{0,-15}{1,25}{2,30}", "1.Post a Tweet  ", "2. My Tweets",
//             "3. View All Tweets");
//         Console.WriteLine("{0,-15}|{1,25}{2,30}", "4.List All users  ", "5. Reset Password", "6. Logout");
//
//         Console.Write("Please provide a  response 1-6: ");
//         var response = Console.ReadLine();
//         switch (response)
//         {
//             case "1":
//             {
//                 PostTweet(_currentUser);
//
//                 UserMenu();
//                 break;
//             }
//             case "2":
//             {
//                 GetMyTweets(_currentUser);
//                 Console.Write("\nPress any key to load dashboard");
//                 UserMenu();
//                 break;
//             }
//             case "3":
//             {
//                 GetAllTweets();
//                 Console.Write("\nPress any key to load dashboard");
//                 UserMenu();
//                 break;
//             }
//             case "4":
//                 var userList = _userRepository.GetAllUsers();
//                 Console.WriteLine("List of all the users Registered: ");
//                 foreach (var user in userList)
//                 {
//                     Console.WriteLine("Name: " + user.FirstName + " " + user.LastName);
//                     Console.WriteLine("Email: " + user.EmailId);
//                     Console.WriteLine("--------");
//                 }
//
//                 Console.WriteLine("-----------------------------------");
//                 Thread.Sleep(2000);
//                 UserMenu();
//                 break;
//             case "5":
//                 ResetPassword();
//                 break;
//             case "6":
//                 UserLogout(_currentUser);
//                 break;
//             default:
//                 Console.WriteLine("--- Invalid Input--- ");
//                 break;
//         }
//     }
//
//     public void UserLogout(string emailId)
//     {
//         Console.WriteLine("Successfully logged out");
//
//         _userRepository.SetLoginStatus(emailId, false);
//         _currentUser = "";
//         _isLoggedIn = false;
//
//         IntroAllUsers();
//     }
//
//     public void ForgetPassword()
//     {
//         while (true)
//         {
//             Welcome();
//             Console.WriteLine("------ FORGOT PASSWORD------ ");
//             Console.WriteLine("Reset using Date of Birth");
//             Console.Write("Enter username(emailID)  :");
//             var user = Console.ReadLine();
//             Console.Write("\nEnter Registered DOB(dd-MM-yyyy)  :");
//             var dob = Console.ReadLine();
//             var dateTime = new DateTime();
//             var validDate = DateTime.TryParseExact(dob, "dd-MM-yyyy", DateTimeFormatInfo.InvariantInfo,
//                 DateTimeStyles.None, out dateTime);
//             if (validDate)
//             {
//                 Console.Write("\nEnter new Password  :");
//                 var pass = Console.ReadLine();
//                 if (pass != null && user != null && _userRepository.ForgetPassword(user, dateTime, pass))
//                 {
//                     Console.WriteLine("\n--- Password has been updated Successfully--- \n");
//                     IntroAllUsers();
//                 }
//                 else
//                 {
//                     Console.WriteLine("-----  Failed to validate Username or DOB. Please Try again ----- ");
//                     continue;
//                 }
//             }
//             else
//             {
//                 Console.WriteLine("--- Invalid Date format--- ");
//                 continue;
//             }
//
//             break;
//         }
//     }
//
//     public void ResetPassword()
//     {
//         Welcome();
//         Console.WriteLine("------ RESET/CHANGE CURRENT PASSWORD ------ \n");
//         Console.Write("Enter username  :");
//         var user = Console.ReadLine();
//         Console.Write("\n Enter current password  :");
//         var oldpassword = Console.ReadLine();
//         Console.Write("\n Enter new password  :");
//         var pass = Console.ReadLine();
//         if (pass != null && oldpassword != null && user != null && _userRepository.ResetPassword(user, oldpassword, pass))
//             Console.WriteLine("\n--- Password UPDATED Successfully--- \n");
//         else
//             Console.WriteLine("--- \n Failed to validate Username or current Password : Try again--- ");
//
//         IntroAllUsers();
//     }
//
//
//     public void PostTweet(string currentUser)
//     {
//         while (true)
//         {
//             Welcome();
//             Console.WriteLine("Logged in as:  " + currentUser);
//             var tweet = new TweetDetails();
//             Console.WriteLine("POST A NEW TWEET");
//             //Console.Write("{1,-10}", "Type Your Tweet:", ":-");
//             Console.Write("Type Your Tweet: :-");
//             tweet.TweetData = Console.ReadLine();
//             if (tweet.TweetData == string.Empty)
//             {
//                 Console.WriteLine("\n--- Nothing to Post--- ");
//                 continue;
//             }
//             if (tweet.TweetData is { Length: > 200 })
//             {
//                 Console.WriteLine("Ah oh! Shorten you message. Tweet length exceeds the maximum character length of 200");
//                 continue;
//             }
//
//             tweet.TweetTime = DateTime.Now;
//             _tweetRepository.PostTweet(tweet, currentUser);
//             Console.WriteLine("\n Tweet Posted Successfully");
//
//             break;
//         }
//     }
//
//     public void GetMyTweets(string emailId)
//     {
//         Welcome();
//         Console.WriteLine("\t\t ------- Your TWEETS --------");
//         var user = new UserDetails();
//         user = _tweetRepository.GetUser(emailId);
//         var tweets = _tweetRepository.GetTweetById(user.EmailId);
//
//         if (tweets.Count > 0)
//         {
//
//
//             foreach (var tweet in tweets)
//             {
//                 Console.WriteLine("---------------------------------------------------");
//                 Console.Write($"Tweeted By : {emailId}");
//                 Console.WriteLine($"Tweeted At : {tweet.TweetTime}");
//                 Console.WriteLine("\t  " + tweet.TweetData);
//             }
//         }
//
//         else
//         {
//             Console.WriteLine("No Tweets found, let's get started");
//             PostTweet(_currentUser);
//         }
//     }
//
//     public void GetAllTweets()
//     {
//         Welcome();
//         Console.WriteLine("ALL PUBLIC TWEETS");
//         var tweets = _tweetRepository.GetAllTweet();
//         foreach (var tweet in tweets)
//         {
//             Console.WriteLine("---------------------------------------------------");
//             Console.Write("\t Tweeted By:  " + tweet.User.FirstName +" "+ tweet.User.LastName);
//             Console.WriteLine("\t Tweeted At:  " + tweet.TweetTime);
//             Console.WriteLine("\t Message: " + tweet.TweetData);
//             Console.WriteLine("\t\t-------------------------------\t");
//         }
//     }
//
//     private static string Gender(string userResponse)
//     {
//         Console.Write($"{"Gender : 1 - Male  2- Female  3- Others",-10}");
//
//         userResponse = Console.ReadLine();
//         if (userResponse == string.Empty)
//         {
//             Console.WriteLine("--- Gender is Mandatory --- ");
//             Gender("");
//             return null;
//         }
//         else
//             switch (userResponse)
//             {
//                 case "1":
//                     return "Male";
//                     //break;
//                 case "2":
//                     return "Female";
//                     //break;
//                 case "3":
//                     return "Others";
//                     //break;
//                 default:
//                     Console.WriteLine(" --- Invalid Response --- ");
//                     Gender("");
//                     return "null";
//                     //break;
//             }
//
//     }
//
//     private bool UserLogin()
//     {
//         Console.Write("Enter username  :");
//         var user = Console.ReadLine();
//         Console.Write("\nEnter Password  :");
//         var pass = Console.ReadLine();
//
//         if (_userRepository.UserLogin(user, pass))
//         {
//             _currentUser = user;
//             _isLoggedIn = true;
//             return true;
//         }
//
//         return false;
//         //return _userRepository.UserLogin(user, pass);
//     }
//
//     private static void ValidateEmail(string data)
//     {
//         var email = data;
//         var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
//         var match = regex.Match(email);
//         if (!match.Success) throw new InvalidEmailException(email);
//     }
//
//
//     private string ReEnterEmail(string data)
//     {
//         var data2 = data;
//         Console.WriteLine("Please re-enter mail Id");
//         data = Console.ReadLine();
//         if (data == string.Empty)
//         {
//             Console.WriteLine("---  EmailID is Mandatory--- ");
//             return "";
//         }
//
//         try
//         {
//             ValidateEmail(data);
//             data2 = data;
//             var flag = _userRepository.DuplicateCheck(data2);
//             if (flag)
//             {
//                 Console.WriteLine("Mail Id already exists!");
//                 data2 = ReEnterEmail("");
//             }
//         }
//         catch (InvalidEmailException ex)
//         {
//             Console.WriteLine(ex.Message);
//             data2 = ReEnterEmail("");
//         }
//
//         return data2;
//     }
// }