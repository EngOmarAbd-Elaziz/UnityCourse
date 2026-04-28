# Comprehensive Fixes and Improvements Plan

This plan details the resolution of the 12 issues requested, ensuring secure authentication, robust data validation, and clean UI/UX without breaking existing functionality.

## User Review Required
> [!IMPORTANT]
> - **Reset Password System**: A new `password_resets` table will be added to SQLite. The email sending will remain *simulated* using latency (as no SMTP server was configured previously), but the OTP logic, validation, and expiration will be fully functional.
> - **Admin Users View**: A new `View` modal will be built dynamically via JS inside `admin.js` to display read-only user details without breaking the existing layout.

## Proposed Changes

### Auth & Session (Logout & Global Guard)
- **`public/js/main.js`**: 
  - Enhance the global auth guard. If a user accesses a protected page without a token, the script will dynamically construct and display the `login-modal` if it's missing from the page DOM.
  - When clicking logout, `localStorage.clear()` (or explicitly removing `profileImage` and auth items) will be enforced before redirecting to `/login`.

### Profile Image Bug
- **`public/js/main.js`**: Clear `profileImage` from `localStorage` on logout.
- **`public/js/profile.js`**: Dynamically read `profile_image` from the backend `/api/auth/profile` request and update the DOM directly, rather than relying on stale `localStorage` values.

### Reset Password Flow (CRITICAL)
- **`database/db.js`**: 
  - [NEW] Add `password_resets` table with `email, otp, expires_at`.
- **`server/routes/auth.js`**: 
  - [MODIFY] Replace simulated forgot-password route with three complete endpoints:
    - `POST /forgot-password` (Generate OTP, save to DB, simulate email send).
    - `POST /verify-otp` (Check OTP and expiration).
    - `POST /reset-password` (Hash new password, update user/company table).
- **`public/login.html`**: 
  - [MODIFY] Add an OTP verification form container and an enter-new-password container.
- **`public/js/login.js`**: 
  - [MODIFY] Handle the step-by-step OTP flow natively.

### Rating System & Course Enrollment
- **`server/routes/data.js`**: 
  - [MODIFY] Verify `/rate` route explicitly binds to `user_id`. Ensure `AVG(rating)` properly calculates.
  - [MODIFY] `POST /user/enroll` must reject requests if `req.userRole === 'admin'`.
- **`public/js/projects.js`**: 
  - [MODIFY] Send `userId` correctly. Reject Admin enrollment on the frontend with a toast.
- **`public/js/admin.js` & `public/js/profile.js`**: 
  - [MODIFY] Display the average rating dynamically (or "Not Rated") when rendering `courses` and `projects`.

### UI Layouts & Profile Links
- **`public/css/style.css`**: 
  - [MODIFY] Transform `.items-grid` to a single-column vertical list with `max-width: 80%` and `margin: 0 auto`. Add padding on the sides.
  - [MODIFY] Center `.nav-links`.
- **`public/js/profile.js`**: 
  - [MODIFY] Ensure `item.title` anchors directly link to the item details page, identical to the action button behavior.

### Delete Modal & Toast Fixes
- **`public/js/profile.js` & `public/js/main.js`**: 
  - [MODIFY] Remove `confirm()` browser alerts. Build a globally accessible `showConfirmModal(msg, onConfirm)` function.
  - [MODIFY] In the Toast utility, implement a `document.querySelectorAll('.toast')` check. If `>= 3`, smoothly remove the oldest. Ensure `pointer-events: none` and full `.remove()` on fade out to prevent invisible overlays.

### Admin View User Modal
- **`public/js/admin.js`**: 
  - [MODIFY] Add a "View" button in `loadUsers()`. Clicking opens a custom modal listing the user's name, email, courses, and projects retrieved via a new backend endpoint.
- **`server/routes/admin.js`**: 
  - [NEW] Endpoint `GET /api/admin/users/:id/details` to aggregate user data.

## Verification Plan

### Automated Tests
- Validate JWT role parsing and OTP expirations manually using Postman or browser tools.

### Manual Verification
- Test login, logout, and forced modal on protected paths.
- Step through the entire Forgot Password -> OTP -> Reset -> Auto-login flow.
- Try enrolling as an admin. Verify rejection.
- Check the layout of courses/projects on Desktop & Mobile.
