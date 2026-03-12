# Rudyson's Autify OIDC Provider

Autify is identity provider for authentication with support of both local and external login systems. Based on microservice architecture, it provides a robust and scalable solution for managing user identities and access control.
- RBAC / ABAC access control.
- JWT token:
    - Short-lived access token.
    - Long-lived refresh token with revoke stored in HttpOnly Cookie.
- DDD architecture.
- User management system.

## Overview

Use this system in your project to enable OpenID Connect (OIDC) authentication with Autify. This provider allows your application to authenticate users via Autify's OIDC service, facilitating secure and seamless user login experiences.

## Features

### Authentication flow

```
Client
   ↓
GET /connect/authorize
   ↓
Login Page (Identity)
   ↓
SignInManager.PasswordSignInAsync
   ↓
Authentication Cookie
   ↓
User.Identity.IsAuthenticated = true
   ↓
/connect/authorize
   ↓
OpenIddict Authorization code
   ↓
POST /connect/token
   ↓
access_token + id_token


Result:
POST /connect/token
grant_type=authorization_code
code=xxxxx
```

### Accounts

#### Local account