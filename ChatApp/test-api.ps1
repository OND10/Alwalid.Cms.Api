# ChatApp API Test Script
# This PowerShell script helps you test the Google OAuth authentication endpoint

Write-Host "🚀 ChatApp API Test Script" -ForegroundColor Green
Write-Host "================================" -ForegroundColor Green
Write-Host ""

# Function to test if API is running
function Test-APIHealth {
    try {
        Write-Host "🔍 Checking if ChatApp API is running..." -ForegroundColor Yellow
        $response = Invoke-RestMethod -Uri "https://localhost:7206/swagger/index.html" -Method Get -SkipCertificateCheck
        Write-Host "✅ API is running on https://localhost:7206" -ForegroundColor Green
        return $true
    }
    catch {
        Write-Host "❌ API is not running or not accessible" -ForegroundColor Red
        Write-Host "Please start the API first with: dotnet run" -ForegroundColor Yellow
        return $false
    }
}

# Function to get Google token instructions
function Show-GoogleTokenInstructions {
    Write-Host ""
    Write-Host "📋 How to get your Google Access Token:" -ForegroundColor Cyan
    Write-Host "1. Go to Google OAuth Playground: https://developers.google.com/oauthplayground/" -ForegroundColor White
    Write-Host "2. Click the gear icon and select 'Use your own OAuth credentials'" -ForegroundColor White
    Write-Host "3. Enter Client ID: 212127799857-ltspi8qut4fu0g94sj3t2kpdps831kgh.apps.googleusercontent.com" -ForegroundColor White
    Write-Host "4. Select scopes: userinfo.email and userinfo.profile" -ForegroundColor White
    Write-Host "5. Authorize and exchange code for tokens" -ForegroundColor White
    Write-Host "6. Copy the Access Token" -ForegroundColor White
    Write-Host ""
}

# Function to test the authentication endpoint
function Test-AuthEndpoint {
    param(
        [string]$GoogleToken
    )
    
    if ([string]::IsNullOrWhiteSpace($GoogleToken)) {
        Write-Host "❌ Google token is required" -ForegroundColor Red
        return
    }

    try {
        Write-Host "🔄 Testing authentication with ChatApp API..." -ForegroundColor Yellow
        
        $headers = @{
            "Content-Type" = "application/json"
            "Accept" = "application/json"
        }
        
        $body = @{
            googleToken = $GoogleToken
        } | ConvertTo-Json
        
        $response = Invoke-RestMethod -Uri "https://localhost:7206/api/auth/google" -Method Post -Headers $headers -Body $body -SkipCertificateCheck
        
        Write-Host "✅ Authentication Successful!" -ForegroundColor Green
        Write-Host ""
        Write-Host "📊 Response Details:" -ForegroundColor Cyan
        Write-Host "User: $($response.user.name) ($($response.user.email))" -ForegroundColor White
        Write-Host "User ID: $($response.user.id)" -ForegroundColor White
        Write-Host "Token Type: $($response.tokenType)" -ForegroundColor White
        Write-Host "Expires In: $($response.expiresIn) seconds" -ForegroundColor White
        Write-Host "Access Token: $($response.accessToken.Substring(0, 50))..." -ForegroundColor White
        Write-Host "Refresh Token: $($response.refreshToken.Substring(0, 50))..." -ForegroundColor White
        Write-Host ""
        
        # Save tokens to file for future use
        $tokenData = @{
            accessToken = $response.accessToken
            refreshToken = $response.refreshToken
            user = $response.user
            expiresIn = $response.expiresIn
            timestamp = Get-Date
        }
        
        $tokenData | ConvertTo-Json | Out-File -FilePath "chatapp-tokens.json" -Encoding UTF8
        Write-Host "💾 Tokens saved to 'chatapp-tokens.json'" -ForegroundColor Green
        
        return $response
    }
    catch {
        Write-Host "❌ Authentication Failed!" -ForegroundColor Red
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
        
        if ($_.Exception.Response) {
            $statusCode = $_.Exception.Response.StatusCode.Value__
            Write-Host "Status Code: $statusCode" -ForegroundColor Red
            
            try {
                $errorResponse = $_.Exception.Response.GetResponseStream()
                $reader = New-Object System.IO.StreamReader($errorResponse)
                $errorBody = $reader.ReadToEnd()
                Write-Host "Error Details: $errorBody" -ForegroundColor Red
            }
            catch {
                Write-Host "Could not read error details" -ForegroundColor Red
            }
        }
    }
}

# Function to test other endpoints with JWT token
function Test-MessagesEndpoint {
    param(
        [string]$JwtToken
    )
    
    if ([string]::IsNullOrWhiteSpace($JwtToken)) {
        Write-Host "❌ JWT token is required" -ForegroundColor Red
        return
    }

    try {
        Write-Host "🔄 Testing messages endpoint..." -ForegroundColor Yellow
        
        $headers = @{
            "Authorization" = "Bearer $JwtToken"
            "Accept" = "application/json"
        }
        
        $response = Invoke-RestMethod -Uri "https://localhost:7206/api/messages/latest?count=5" -Method Get -Headers $headers -SkipCertificateCheck
        
        Write-Host "✅ Messages endpoint working!" -ForegroundColor Green
        Write-Host "Retrieved $($response.Count) messages" -ForegroundColor White
        
        foreach ($message in $response) {
            Write-Host "- $($message.user.name): $($message.content)" -ForegroundColor Gray
        }
    }
    catch {
        Write-Host "❌ Messages endpoint failed!" -ForegroundColor Red
        Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

# Main execution
if (-not (Test-APIHealth)) {
    exit 1
}

Show-GoogleTokenInstructions

# Prompt for Google token
$googleToken = Read-Host "🔑 Please enter your Google Access Token"

if ([string]::IsNullOrWhiteSpace($googleToken)) {
    Write-Host "❌ No token provided. Exiting." -ForegroundColor Red
    exit 1
}

# Test authentication
$authResponse = Test-AuthEndpoint -GoogleToken $googleToken

if ($authResponse) {
    # Test messages endpoint with JWT token
    Write-Host ""
    Write-Host "🧪 Testing authenticated endpoints..." -ForegroundColor Cyan
    Test-MessagesEndpoint -JwtToken $authResponse.accessToken
}

Write-Host ""
Write-Host "🎉 Test completed!" -ForegroundColor Green
Write-Host "Check 'chatapp-tokens.json' for saved authentication tokens." -ForegroundColor Yellow

# Pause to keep window open
Write-Host ""
Write-Host "Press any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")