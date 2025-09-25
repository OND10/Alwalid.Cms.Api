#!/bin/bash

# ChatApp API Test Script
# This bash script helps you test the Google OAuth authentication endpoint

echo "🚀 ChatApp API Test Script"
echo "================================"
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
CYAN='\033[0;36m'
WHITE='\033[1;37m'
NC='\033[0m' # No Color

# Function to test if API is running
test_api_health() {
    echo -e "${YELLOW}🔍 Checking if ChatApp API is running...${NC}"
    
    if curl -s -k "https://localhost:7206/swagger/index.html" > /dev/null 2>&1; then
        echo -e "${GREEN}✅ API is running on https://localhost:7206${NC}"
        return 0
    else
        echo -e "${RED}❌ API is not running or not accessible${NC}"
        echo -e "${YELLOW}Please start the API first with: dotnet run${NC}"
        return 1
    fi
}

# Function to show Google token instructions
show_google_token_instructions() {
    echo ""
    echo -e "${CYAN}📋 How to get your Google Access Token:${NC}"
    echo -e "${WHITE}1. Go to Google OAuth Playground: https://developers.google.com/oauthplayground/${NC}"
    echo -e "${WHITE}2. Click the gear icon and select 'Use your own OAuth credentials'${NC}"
    echo -e "${WHITE}3. Enter Client ID: 212127799857-ltspi8qut4fu0g94sj3t2kpdps831kgh.apps.googleusercontent.com${NC}"
    echo -e "${WHITE}4. Select scopes: userinfo.email and userinfo.profile${NC}"
    echo -e "${WHITE}5. Authorize and exchange code for tokens${NC}"
    echo -e "${WHITE}6. Copy the Access Token${NC}"
    echo ""
    echo -e "${CYAN}📝 Alternative: Open test-frontend.html in your browser${NC}"
    echo ""
}

# Function to test the authentication endpoint
test_auth_endpoint() {
    local google_token="$1"
    
    if [ -z "$google_token" ]; then
        echo -e "${RED}❌ Google token is required${NC}"
        return 1
    fi

    echo -e "${YELLOW}🔄 Testing authentication with ChatApp API...${NC}"
    
    local response
    local http_code
    
    # Make the API call
    response=$(curl -s -k -w "HTTPSTATUS:%{http_code}" \
        -X POST "https://localhost:7206/api/auth/google" \
        -H "Content-Type: application/json" \
        -H "Accept: application/json" \
        -d "{\"googleToken\": \"$google_token\"}")
    
    # Extract HTTP status code
    http_code=$(echo "$response" | tr -d '\n' | sed -e 's/.*HTTPSTATUS://')
    # Extract response body
    response_body=$(echo "$response" | sed -e 's/HTTPSTATUS:.*//g')
    
    if [ "$http_code" = "200" ]; then
        echo -e "${GREEN}✅ Authentication Successful!${NC}"
        echo ""
        echo -e "${CYAN}📊 Response Details:${NC}"
        
        # Parse JSON response (requires jq, but provide fallback)
        if command -v jq > /dev/null 2>&1; then
            local user_name=$(echo "$response_body" | jq -r '.user.name')
            local user_email=$(echo "$response_body" | jq -r '.user.email')
            local user_id=$(echo "$response_body" | jq -r '.user.id')
            local token_type=$(echo "$response_body" | jq -r '.tokenType')
            local expires_in=$(echo "$response_body" | jq -r '.expiresIn')
            local access_token=$(echo "$response_body" | jq -r '.accessToken')
            local refresh_token=$(echo "$response_body" | jq -r '.refreshToken')
            
            echo -e "${WHITE}User: $user_name ($user_email)${NC}"
            echo -e "${WHITE}User ID: $user_id${NC}"
            echo -e "${WHITE}Token Type: $token_type${NC}"
            echo -e "${WHITE}Expires In: $expires_in seconds${NC}"
            echo -e "${WHITE}Access Token: ${access_token:0:50}...${NC}"
            echo -e "${WHITE}Refresh Token: ${refresh_token:0:50}...${NC}"
            
            # Save tokens to file
            echo "$response_body" | jq '.' > chatapp-tokens.json
            echo -e "${GREEN}💾 Tokens saved to 'chatapp-tokens.json'${NC}"
            
            # Return the access token for further testing
            echo "$access_token"
        else
            echo -e "${WHITE}Raw Response: $response_body${NC}"
            echo -e "${YELLOW}Install 'jq' for better JSON parsing${NC}"
            
            # Save raw response
            echo "$response_body" > chatapp-tokens.json
            echo -e "${GREEN}💾 Response saved to 'chatapp-tokens.json'${NC}"
        fi
        
        return 0
    else
        echo -e "${RED}❌ Authentication Failed!${NC}"
        echo -e "${RED}HTTP Status Code: $http_code${NC}"
        echo -e "${RED}Error Response: $response_body${NC}"
        return 1
    fi
}

# Function to test messages endpoint
test_messages_endpoint() {
    local jwt_token="$1"
    
    if [ -z "$jwt_token" ]; then
        echo -e "${RED}❌ JWT token is required${NC}"
        return 1
    fi

    echo -e "${YELLOW}🔄 Testing messages endpoint...${NC}"
    
    local response
    local http_code
    
    response=$(curl -s -k -w "HTTPSTATUS:%{http_code}" \
        -X GET "https://localhost:7206/api/messages/latest?count=5" \
        -H "Authorization: Bearer $jwt_token" \
        -H "Accept: application/json")
    
    http_code=$(echo "$response" | tr -d '\n' | sed -e 's/.*HTTPSTATUS://')
    response_body=$(echo "$response" | sed -e 's/HTTPSTATUS:.*//g')
    
    if [ "$http_code" = "200" ]; then
        echo -e "${GREEN}✅ Messages endpoint working!${NC}"
        
        if command -v jq > /dev/null 2>&1; then
            local message_count=$(echo "$response_body" | jq 'length')
            echo -e "${WHITE}Retrieved $message_count messages${NC}"
            
            # Display messages
            echo "$response_body" | jq -r '.[] | "- " + .user.name + ": " + .content'
        else
            echo -e "${WHITE}Raw Response: $response_body${NC}"
        fi
    else
        echo -e "${RED}❌ Messages endpoint failed!${NC}"
        echo -e "${RED}HTTP Status Code: $http_code${NC}"
        echo -e "${RED}Error Response: $response_body${NC}"
    fi
}

# Function to create a simple curl example
create_curl_example() {
    local google_token="$1"
    
    echo ""
    echo -e "${CYAN}📋 Complete curl example:${NC}"
    echo ""
    cat << EOF
curl -X POST "https://localhost:7206/api/auth/google" \\
     -H "Content-Type: application/json" \\
     -H "Accept: application/json" \\
     -d '{"googleToken": "$google_token"}' \\
     -k
EOF
    echo ""
}

# Main execution
main() {
    # Test if API is running
    if ! test_api_health; then
        exit 1
    fi

    # Show instructions
    show_google_token_instructions

    # Prompt for Google token
    echo -n "🔑 Please enter your Google Access Token: "
    read -r google_token

    if [ -z "$google_token" ]; then
        echo -e "${RED}❌ No token provided. Exiting.${NC}"
        exit 1
    fi

    # Create curl example
    create_curl_example "$google_token"

    # Test authentication
    echo -e "${CYAN}🧪 Testing authentication...${NC}"
    jwt_token=$(test_auth_endpoint "$google_token")

    if [ $? -eq 0 ] && [ -n "$jwt_token" ]; then
        echo ""
        echo -e "${CYAN}🧪 Testing authenticated endpoints...${NC}"
        test_messages_endpoint "$jwt_token"
    fi

    echo ""
    echo -e "${GREEN}🎉 Test completed!${NC}"
    echo -e "${YELLOW}Check 'chatapp-tokens.json' for saved authentication tokens.${NC}"
}

# Make script executable and run
chmod +x "$0"
main "$@"