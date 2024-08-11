import { UserManager, WebStorageStateStore } from 'oidc-client-ts';

const config = {
    authority: "https://localhost:5275",  
    client_id: "react_client_app",    
    redirect_uri: "http://localhost:3000/callback",
    post_logout_redirect_uri: "http://localhost:3000/",
    response_type: "code",
    scope: "openid email profile api",
    userStore: new WebStorageStateStore({ store: window.localStorage })
};

const userManager = new UserManager(config);

export function login() {
    return userManager.signinRedirect();
}

export function logout() {
    return userManager.signoutRedirect();
}

export function getUser() {
    return userManager.getUser();
}

export default userManager;