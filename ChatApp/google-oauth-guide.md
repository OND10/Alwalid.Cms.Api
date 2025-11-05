# 🔐 Complete Guide: Getting Google Token for ChatApp API

## Overview

To use the `POST /api/auth/google` endpoint, you need a **Google Access Token**. This guide shows multiple ways to obtain it.

## Method 1: Google OAuth Playground (Quickest for Testing)

### Step 1: Go to Google OAuth Playground
1. Open [Google OAuth 2.0 Playground](https://developers.google.com/oauthplayground/)
2. Click the ⚙️ gear icon (OAuth 2.0 configuration)
3. Check "Use your own OAuth credentials"
4. Enter your OAuth credentials:
   - **OAuth Client ID**: `212127799857-ltspi8qut4fu0g94sj3t2kpdps831kgh.apps.googleusercontent.com`
   - **OAuth Client Secret**: `GOCSPX-YOUR_CLIENT_SECRET_HERE`

### Step 2: Select Scopes
1. In the left panel, find "Google+ API v1"
2. Select these scopes:
   - `https://www.googleapis.com/auth/userinfo.email`
   - `https://www.googleapis.com/auth/userinfo.profile`
3. Click "Authorize APIs"

### Step 3: Complete Authorization
1. Sign in with your Google account
2. Grant permissions to the application
3. You'll be redirected back to the playground

### Step 4: Get Access Token
1. Click "Exchange authorization code for tokens"
2. Copy the **Access Token** from the response
3. This token can be used with the ChatApp API

### Step 5: Test with ChatApp API
```bash
curl -X POST "https://localhost:7206/api/auth/google" \
     -H "Content-Type: application/json" \
     -H "Accept: application/json" \
     -d '{"googleToken": "YOUR_ACCESS_TOKEN_HERE"}'
```

---

## Method 2: Frontend JavaScript Implementation

### Step 1: Set up Google Sign-In HTML
Use the `test-frontend.html` file provided in the project root.

### Step 2: Serve the HTML File
```bash
# Option 1: Using Python
python -m http.server 8080

# Option 2: Using Node.js (if you have it)
npx http-server

# Option 3: Using .NET
dotnet tool install --global dotnet-serve
dotnet serve -o -p 8080
```

### Step 3: Open in Browser
1. Navigate to `http://localhost:8080/test-frontend.html`
2. Click "Sign in with Google"
3. Complete the OAuth flow
4. Copy the access token
5. Test the API directly from the page

---

## Method 3: Using Postman

### Step 1: Set up OAuth 2.0 in Postman
1. Create a new request in Postman
2. Go to the **Authorization** tab
3. Select **OAuth 2.0** as the type
4. Configure the following:
   - **Grant Type**: Authorization Code
   - **Callback URL**: `https://localhost:7206/signin-google`
   - **Auth URL**: `https://accounts.google.com/o/oauth2/auth`
   - **Access Token URL**: `https://oauth2.googleapis.com/token`
   - **Client ID**: `212127799857-ltspi8qut4fu0g94sj3t2kpdps831kgh.apps.googleusercontent.com`
   - **Client Secret**: `GOCSPX-YOUR_CLIENT_SECRET_HERE`
   - **Scope**: `https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile`

### Step 2: Get Token
1. Click "Get New Access Token"
2. Complete the OAuth flow in the browser
3. Copy the access token

### Step 3: Test ChatApp API
1. Create a new POST request to `https://localhost:7206/api/auth/google`
2. Set Content-Type to `application/json`
3. Set the request body:
```json
{
    "googleToken": "YOUR_ACCESS_TOKEN_HERE"
}
```

---

## Method 4: React/JavaScript Frontend

### Install Google Identity Services
```bash
npm install google-auth-library
```

### React Component Example
```jsx
import React, { useState } from 'react';

const GoogleAuth = () => {
    const [accessToken, setAccessToken] = useState(null);
    const [user, setUser] = useState(null);

    useEffect(() => {
        // Load Google Identity Services
        const script = document.createElement('script');
        script.src = 'https://accounts.google.com/gsi/client';
        script.async = true;
        script.defer = true;
        document.head.appendChild(script);

        script.onload = () => {
            window.google.accounts.id.initialize({
                client_id: '212127799857-ltspi8qut4fu0g94sj3t2kpdps831kgh.apps.googleusercontent.com',
                callback: handleCredentialResponse
            });
        };
    }, []);

    const handleCredentialResponse = (response) => {
        // Get access token
        window.google.accounts.oauth2.initTokenClient({
            client_id: '212127799857-ltspi8qut4fu0g94sj3t2kpdps831kgh.apps.googleusercontent.com',
            scope: 'https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email',
            callback: (tokenResponse) => {
                setAccessToken(tokenResponse.access_token);
                authenticateWithChatApp(tokenResponse.access_token);
            }
        }).requestAccessToken();
    };

    const authenticateWithChatApp = async (googleToken) => {
        try {
            const response = await fetch('https://localhost:7206/api/auth/google', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ googleToken })
            });

            const data = await response.json();
            
            if (response.ok) {
                // Store JWT tokens
                localStorage.setItem('accessToken', data.accessToken);
                localStorage.setItem('refreshToken', data.refreshToken);
                setUser(data.user);
            } else {
                console.error('Authentication failed:', data.error);
            }
        } catch (error) {
            console.error('API call failed:', error);
        }
    };

    const signIn = () => {
        window.google.accounts.id.prompt();
    };

    return (
        <div>
            {!user ? (
                <button onClick={signIn}>Sign in with Google</button>
            ) : (
                <div>
                    <h3>Welcome, {user.name}!</h3>
                    <p>Email: {user.email}</p>
                    <p>Access Token: {accessToken?.substring(0, 20)}...</p>
                </div>
            )}
        </div>
    );
};
```

---

## Method 5: cURL with Manual Token

### Step 1: Get Token from Google OAuth Playground
Follow Method 1 to get your access token.

### Step 2: Use cURL
```bash
# Replace YOUR_ACCESS_TOKEN with the actual token
curl -X POST "https://localhost:7206/api/auth/google" \
     -H "Content-Type: application/json" \
     -H "Accept: application/json" \
     -d '{
       "googleToken": "YOUR_ACCESS_TOKEN_HERE"
     }' \
     -k  # Use -k to ignore SSL certificate issues in development
```

### Expected Response
```json
{
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "d1f2e3c4b5a6...",
    "tokenType": "Bearer",
    "expiresIn": 3600,
    "user": {
        "id": "123e4567-e89b-12d3-a456-426614174000",
        "email": "user@example.com",
        "name": "John Doe",
        "profilePictureUrl": "https://lh3.googleusercontent.com/..."
    }
}
```

---

## 🚨 Important Notes

### Security Considerations
1. **Never expose Client Secret** in frontend code
2. **Use HTTPS** in production
3. **Validate tokens** on the server side
4. **Implement token refresh** logic

### Token Expiration
- Google access tokens typically expire after **1 hour**
- Implement refresh logic using the refresh token from ChatApp API
- Use `POST /api/auth/refresh` endpoint with the refresh token

### CORS Configuration
Make sure your frontend domain is added to CORS policy in `Program.cs`:
```csharp
policy.WithOrigins("http://localhost:3000", "http://localhost:8080")
```

### Troubleshooting

#### Common Issues:
1. **"Invalid token"** - Token might be expired, get a fresh one
2. **CORS errors** - Add your frontend URL to CORS policy
3. **SSL errors** - Use `-k` flag with curl or configure HTTPS properly
4. **Client ID mismatch** - Ensure you're using the correct Google Client ID

#### Debug Steps:
1. Check if ChatApp API is running: `https://localhost:7206/swagger`
2. Verify Google token at: `https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=YOUR_TOKEN`
3. Check browser console for JavaScript errors
4. Verify CORS headers in network tab

---

## 🎯 Quick Start Guide

1. **Fastest way for testing**: Use Google OAuth Playground (Method 1)
2. **For frontend development**: Use the provided HTML test page (Method 2)
3. **For API testing**: Use Postman (Method 3)
4. **For production**: Implement proper frontend integration (Method 4)

Choose the method that best fits your needs and follow the step-by-step instructions above!