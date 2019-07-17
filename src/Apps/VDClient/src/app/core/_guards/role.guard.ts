import { Injectable } from '@angular/core';
import {
    Router,
    CanActivate,
    ActivatedRouteSnapshot
} from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import decode from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
    constructor(public auth: AuthenticationService, public router: Router) { }
    canActivate(route: ActivatedRouteSnapshot): boolean {

        const currentUser = this.auth.currentUserValue;
        if (currentUser) {
            // logged in so return true

            // this will be passed from the route config
            // on the data property
            const expectedRole = route.data.expectedRole;
            const token = currentUser.token;
            // decode the token to get its payload
            const tokenPayload = decode(token);

            console.log(tokenPayload);

            if (tokenPayload.role !== expectedRole) {
                this.router.navigate(['login']);
                return false;
            } else {
                return true;
            }
        }

        return false;
    }
}
