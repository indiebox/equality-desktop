# UserService

The service is responsible for actions related to the user.

{% hint style="info" %}
This service implements the `IUserService`. You can use the [Dependency Injection](../general/catel-framework/dependency-injection.md) for getting instance of class.
{% endhint %}

{% hint style="warning" %}
If errors occur, methods accessing the API will throws exceptions, which you can read more about here. These exceptions will contain a dictionary with the keys and values specified in the API documentation, links to which you can find in the description of the methods below.
{% endhint %}

## LoginAsync

Sends a request to the API to log in to the account. Accepts two parameters as input:

* <mark style="color:blue;">`string`</mark> `email` - user email
* <mark style="color:blue;">`string`</mark> `password` - user password

Returns an object of [`User`](../models/user.md) class and <mark style="color:blue;">`string`</mark> `token.`

API Endpoint: https://equality.com/api/v1/login

Usage example:

```csharp
var CurrentUser = new User();
string CurrentToken;
string CredentialsError;

private async Task OnLoginExecuteAsync()
{
    string Email = "test@mail.ru";
    string Password = "123456";

    try {
        var (user, token) = await UserService.LoginAsync(Email, Password);
        CurrentUser = user;
        CurrentToken = token;
    } catch (UnprocessableEntityHttpException e) {
        CredintialsError = errors.ContainsKey("credentials") ? string.Join("", errors["credentials"]) : string.Empty;
    } catch (HttpRequestException e) {
        CredintialsError = e.ToString();
    }
```

## LoadAuthUserAsync

Sends the request to get an authenticated user to the API.&#x20;

Gets token from `IStateManager.ApiToken`.&#x20;

After success response sets `IStateManager.CurrentUser`.

Returns the server response.

API Endpoint: https://equality.com/api/v1/user

Usage example:

```csharp
private async void CheckAuthUser(string token)
{
    StateManager.ApiToken = token;

    try {
        await UserService.LoadAuthUserAsync();

        // Users token is valid
    } catch (UnauthorizedHttpException) {
        // Users token is invalid
    } catch (HttpRequestException e) {
        // Other error
    }
}
```

## LogoutAsync

Sends the logout authenticated user request to the API.

Gets token from `IStateManager.ApiToken`.&#x20;

After success logout sets `IStateManager.CurrentUser = null` and `IStateManager.ApiToken = null`.

Returns the API response.

Usage example:

```csharp
private async Task OnLogoutExecute() {
    try {
        await UserService.LogoutAsync();

        // User is logged out
    } catch (HttpRequestException e) {
        // Errors
    }
}
```

## SendResetPasswordTokenAsync

Sends a request to send a message with a password recovery code to the mail on the API. Accepts one parameter as input:

* <mark style="color:blue;">`string`</mark>` ``email` - user email

Returns the server response.

API Endpoint: https://equality.com/api/v1/forgot-password

Usage example:

```csharp
string Error;

private async Task OnOpenResetPasswordPageExecute()
{
    try {
        string Email = "test@mail.ru";
        var response = await UserService.ForgotPasswordEmailSendAsync(Email);
    } catch (UnprocessableEntityHttpException e) {
        var errors = e.Errors;
        Error = errors.ContainsKey("email") ? string.Join("", errors["email"]) : string.Empty;
    } catch (HttpRequestException e) {
        Error = e.ToString();
    }
}
```

## ResetPasswordAsync

Sends a request to the API to reset the password. Accepts four parameters as input:

* <mark style="color:blue;">`string`</mark>` ``email` - user email
* <mark style="color:blue;">`string`</mark>` ``password` - user password
* <mark style="color:blue;">`string`</mark>` ``passwordConfirmation` - repeat password
* <mark style="color:blue;">`string`</mark>` ``token` - user token from mail

Returns the server response.

API Endpoint: https://equality.com/api/v1/reset-password

Usage example:

```csharp
string Email;
string Password;
string PasswordConfirmation;
string Token;
string EmailError;

private async Task OnOpenLoginPageExecute()
{
    try {
        var response = await UserServise.ResetPasswordAsync(Email, Password, PasswordConfirmation, Token);
    } catch (UnprocessableEntityHttpException e) {
        var errors = e.Errors;
        EmailError = errors.ContainsKey("email") ? string.Join("", errors["email"]) : string.Empty;
    } catch (HttpRequestException e) {
        EmailError  = e.ToString();
    }
}
```

## Deserialize

Deserializes the JSON string to `User` instance.

Accepts one parameter as input:

* <mark style="color:blue;">`string`</mark>` ``data` - json string with user data

Returns the `User` instance.

Usage example:

```csharp
var response = await ApiClient.PostAsync("login", data);
User user = Deserialize(response.Content["data"].ToString());
```

