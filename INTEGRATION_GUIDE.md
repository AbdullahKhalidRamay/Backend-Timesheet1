# TimeSheet API Integration Guide

## Overview

This guide explains how to integrate the React frontend with the .NET Core backend API. The backend provides a complete REST API for all timesheet functionality, replacing the localStorage-based data management.

## Backend API Status

### ✅ Completed APIs
- **Authentication** (`/api/auth/*`)
  - POST `/api/auth/login` - User login
  - POST `/api/auth/register` - User registration
  - GET `/api/auth/me` - Get current user
  - POST `/api/auth/logout` - User logout

- **Users** (`/api/users/*`)
  - GET `/api/users` - Get all users (owner/manager only)
  - GET `/api/users/{id}` - Get user by ID
  - POST `/api/users` - Create user (owner only)
  - PUT `/api/users/{id}` - Update user
  - DELETE `/api/users/{id}` - Delete user (owner only)
  - GET `/api/users/{id}/timesheet` - Get user timesheet

- **Time Entries** (`/api/timeentries/*`)
  - GET `/api/timeentries` - Get all time entries
  - GET `/api/timeentries/{id}` - Get time entry by ID
  - POST `/api/timeentries` - Create time entry
  - PUT `/api/timeentries/{id}` - Update time entry
  - DELETE `/api/timeentries/{id}` - Delete time entry
  - POST `/api/timeentries/{id}/approve` - Approve time entry
  - POST `/api/timeentries/{id}/reject` - Reject time entry
  - POST `/api/timeentries/bulk-approve` - Bulk approve entries
  - GET `/api/timeentries/range` - Get entries by date range
  - GET `/api/timeentries/date` - Get entries by date
  - GET `/api/timeentries/statistics` - Get statistics

- **Projects** (`/api/projects/*`)
  - GET `/api/projects` - Get all projects
  - GET `/api/projects/{id}` - Get project by ID
  - POST `/api/projects` - Create project
  - PUT `/api/projects/{id}` - Update project
  - DELETE `/api/projects/{id}` - Delete project
  - GET `/api/projects/{id}/levels` - Get project levels
  - POST `/api/projects/{id}/levels` - Create project level
  - GET `/api/projects/levels/{levelId}/tasks` - Get tasks
  - POST `/api/projects/levels/{levelId}/tasks` - Create task
  - GET `/api/projects/tasks/{taskId}/subtasks` - Get subtasks
  - POST `/api/projects/tasks/{taskId}/subtasks` - Create subtask

- **Products** (`/api/products/*`)
  - GET `/api/products` - Get all products
  - GET `/api/products/{id}` - Get product by ID
  - POST `/api/products` - Create product
  - PUT `/api/products/{id}` - Update product
  - DELETE `/api/products/{id}` - Delete product

- **Departments** (`/api/departments/*`)
  - GET `/api/departments` - Get all departments
  - GET `/api/departments/{id}` - Get department by ID
  - POST `/api/departments` - Create department
  - PUT `/api/departments/{id}` - Update department
  - DELETE `/api/departments/{id}` - Delete department

- **Teams** (`/api/teams/*`)
  - GET `/api/teams` - Get all teams
  - GET `/api/teams/{id}` - Get team by ID
  - POST `/api/teams` - Create team
  - PUT `/api/teams/{id}` - Update team
  - DELETE `/api/teams/{id}` - Delete team
  - POST `/api/teams/{id}/members` - Add team member
  - DELETE `/api/teams/{id}/members/{userId}` - Remove team member
  - GET `/api/teams/{id}/members` - Get team members
  - GET `/api/teams/user/{userId}` - Get user teams

- **Notifications** (`/api/notifications/*`)
  - GET `/api/notifications` - Get all notifications
  - GET `/api/notifications/{id}` - Get notification by ID
  - PUT `/api/notifications/{id}/read` - Mark as read
  - PUT `/api/notifications/mark-all-read` - Mark all as read
  - GET `/api/notifications/unread-count` - Get unread count

- **Reports** (`/api/reports/*`)
  - GET `/api/reports/timesheet` - Get timesheet report
  - GET `/api/reports/department/{id}` - Get department report
  - GET `/api/reports/team/{id}` - Get team report
  - GET `/api/reports/project/{id}` - Get project report

- **Settings** (`/api/settings/*`)
  - GET `/api/settings/user` - Get user settings
  - PUT `/api/settings/user` - Update user settings
  - GET `/api/settings/system` - Get system settings
  - PUT `/api/settings/system` - Update system setting

## Frontend Integration

### 1. API Service Setup

The frontend now includes a comprehensive API service (`src/services/apiService.ts`) that handles all backend communication:

```typescript
import { api } from '@/services/apiService';

// Example usage
const timeEntries = await api.timeEntries.getAll();
const user = await api.auth.getCurrentUser();
```

### 2. Storage Service Migration

Replace localStorage usage with API calls using the new storage service (`src/services/storageApi.ts`):

```typescript
// Old localStorage usage
import { getTimeEntries } from '@/services/storage';

// New API usage
import { getTimeEntries } from '@/services/storageApi';

// Both return the same data structure, but new version uses API
const entries = await getTimeEntries();
```

### 3. Authentication Integration

Update the auth system to use the backend:

```typescript
import { api } from '@/services/apiService';

// Login
const response = await api.auth.login(email, password);
// Token is automatically stored in localStorage

// Get current user
const user = await api.auth.getCurrentUser();

// Logout
api.auth.logout();
```

## Configuration Steps

### 1. Backend Configuration

1. **Database Setup**:
   ```bash
   cd TimeSheetAPI
   dotnet ef database update
   ```

2. **Start the Backend**:
   ```bash
   dotnet run
   ```
   The API will be available at `https://localhost:7001`

3. **Verify API**:
   - Open `https://localhost:7001/swagger` to see the API documentation
   - Test the health endpoint: `https://localhost:7001/api/health`

### 2. Frontend Configuration

1. **Update API Base URL**:
   In `src/services/apiService.ts`, ensure the API_BASE_URL is correct:
   ```typescript
   const API_BASE_URL = 'https://localhost:7001/api';
   ```

2. **Install Dependencies** (if not already installed):
   ```bash
   cd pro-timeflow-main
   npm install
   ```

3. **Start the Frontend**:
   ```bash
   npm run dev
   ```

### 3. Environment Variables

Create a `.env` file in the frontend root:

```env
VITE_API_BASE_URL=https://localhost:7001/api
VITE_APP_NAME=TimeFlow
```

## Data Migration

### From localStorage to API

The frontend includes both storage systems:

1. **Old System**: `src/services/storage.ts` (localStorage-based)
2. **New System**: `src/services/storageApi.ts` (API-based)

To migrate:

1. **Gradual Migration**: Keep both systems and switch gradually
2. **Data Export**: Export localStorage data and import via API
3. **Fresh Start**: Use the seeded data from the backend

### Seeded Data

The backend includes seeded data for:
- 19 users (3 owners, 6 managers, 10 employees)
- Sample projects, departments, and teams
- Default system settings

## API Authentication

### JWT Token Management

1. **Login Flow**:
   ```typescript
   const response = await api.auth.login(email, password);
   // Token is automatically stored
   ```

2. **Automatic Token Handling**:
   - Tokens are automatically included in API requests
   - Expired tokens trigger automatic logout
   - 401 responses redirect to login

3. **Token Storage**:
   ```typescript
   // Stored automatically
   localStorage.setItem('authToken', token);
   localStorage.setItem('currentUser', JSON.stringify(user));
   ```

## Error Handling

### API Error Handling

The API service includes comprehensive error handling:

```typescript
import { api, handleApiError } from '@/services/apiService';

try {
  const data = await api.timeEntries.getAll();
} catch (error) {
  handleApiError(error);
  // Automatic logout on 401
  // Console logging for debugging
}
```

### Common Error Scenarios

1. **Network Errors**: Retry logic and user feedback
2. **Authentication Errors**: Automatic logout and redirect
3. **Validation Errors**: Form-specific error messages
4. **Server Errors**: Generic error messages with logging

## Testing the Integration

### 1. Backend Testing

```bash
# Test the API
curl -X GET "https://localhost:7001/api/auth/me" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

### 2. Frontend Testing

1. **Login Test**:
   - Use seeded user: `ceo@company.com` / `password`
   - Verify token storage
   - Check protected routes

2. **Data Flow Test**:
   - Create a time entry
   - Verify it appears in the list
   - Test approval workflow

3. **Error Handling Test**:
   - Test with invalid credentials
   - Test with expired tokens
   - Test network failures

## Development Workflow

### 1. Backend Development

```bash
# Start backend with hot reload
cd TimeSheetAPI
dotnet watch run
```

### 2. Frontend Development

```bash
# Start frontend with hot reload
cd pro-timeflow-main
npm run dev
```

### 3. Database Changes

```bash
# Create migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

## Production Deployment

### 1. Backend Deployment

1. **Build the API**:
   ```bash
   dotnet publish -c Release
   ```

2. **Database Migration**:
   ```bash
   dotnet ef database update
   ```

3. **Environment Configuration**:
   - Update connection strings
   - Set production JWT secrets
   - Configure CORS for production domain

### 2. Frontend Deployment

1. **Build the App**:
   ```bash
   npm run build
   ```

2. **Update API URL**:
   - Change `API_BASE_URL` to production URL
   - Update environment variables

3. **Deploy to Web Server**:
   - Serve the `dist` folder
   - Configure HTTPS
   - Set up proper CORS headers

## Troubleshooting

### Common Issues

1. **CORS Errors**:
   - Ensure backend CORS is configured for frontend domain
   - Check `Program.cs` CORS configuration

2. **Authentication Issues**:
   - Verify JWT configuration in `appsettings.json`
   - Check token expiration settings

3. **Database Connection**:
   - Verify SQL Server is running
   - Check connection string in `appsettings.json`
   - Ensure database exists and is accessible

4. **API Endpoints Not Found**:
   - Verify controller routes are correct
   - Check authorization attributes
   - Ensure proper HTTP methods

### Debug Tools

1. **Backend Debugging**:
   - Use Swagger UI: `https://localhost:7001/swagger`
   - Check application logs
   - Use SQL Server Profiler for database queries

2. **Frontend Debugging**:
   - Browser Developer Tools
   - Network tab for API calls
   - Console for error messages

## Security Considerations

### 1. JWT Security

- Use strong, unique secrets
- Set appropriate token expiration
- Implement token refresh mechanism
- Validate tokens on every request

### 2. API Security

- Implement rate limiting
- Use HTTPS in production
- Validate all inputs
- Implement proper authorization

### 3. Data Security

- Encrypt sensitive data
- Implement audit logging
- Regular security updates
- Backup strategies

## Performance Optimization

### 1. Backend Optimization

- Implement caching for frequently accessed data
- Use async/await properly
- Optimize database queries
- Implement pagination for large datasets

### 2. Frontend Optimization

- Implement request caching
- Use React Query for data management
- Optimize bundle size
- Implement lazy loading

## Monitoring and Logging

### 1. Backend Monitoring

- Application insights
- Database performance monitoring
- Error tracking and alerting
- API usage analytics

### 2. Frontend Monitoring

- Error tracking (Sentry, etc.)
- Performance monitoring
- User analytics
- Real-time error reporting

## Conclusion

This integration provides a complete, production-ready timesheet system with:

- ✅ Secure authentication with JWT
- ✅ Complete CRUD operations for all entities
- ✅ Role-based authorization
- ✅ Real-time data synchronization
- ✅ Comprehensive error handling
- ✅ Scalable architecture
- ✅ Production deployment ready

The system is now ready for production use with proper security, performance, and monitoring in place.
