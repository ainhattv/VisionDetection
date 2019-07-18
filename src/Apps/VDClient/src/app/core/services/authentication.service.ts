import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '../../../environments/environment';
import { User } from '../../shared/models/user.model';
import { UserRegister } from '../../shared/models/userRegister.model';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable(); // Create new Observable from behaviorSubject
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  public register(userRegister: UserRegister) {

    if (!userRegister) {
      return;
    }

    return this.http.post<any>(`${environment.authApi}api/v1/Identity`, userRegister)
      .pipe(map(user => {
        return user;
      }));
  }

  public emailConfirmation(userid: string, code: string) {
    const requestmodel = {
      userId: userid,
      code,
    };

    return this.http.post<any>(`${environment.authApi}api/v1/Identity/ConfirmEmail/${userid}`, requestmodel)
      .pipe(map(result => {
        return result;
      }));
  }

  public login(username: string, password: string) {

    const requestmodel = {
      userName: username,
      passWord: password,
    };

    return this.http.post<any>(`${environment.authApi}api/v1/Identity/Login`, requestmodel)
      .pipe(map(user => {
        // login successful if there's a jwt token in the response
        if (user && user.token && user.succeeded === true) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem('currentUser', JSON.stringify(user));
          this.currentUserSubject.next(user);
        }

        return user;
      }));
  }

  logout() {
    // remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}
