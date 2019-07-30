import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, first } from 'rxjs/operators';

import { AuthenticationService } from './authentication.service';

import { WorkPlace } from '../../shared/models/workplace.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WorkplaceService {
  public workplace: WorkPlace;

  constructor(
    private httpClient: HttpClient,
    private authenticationService: AuthenticationService) {
  }

  public loadWorkPlace(userid: string) {
    return this.httpClient.get<WorkPlace>(`${environment.wpsApi}api/v1/WorkPlaces/${userid}`)
      .pipe(map(wp => {
        this.workplace = wp;
        console.log(wp);
        return wp;
      }));
  }

  public createWorkPlace(wpname: string) {
    const requestModel = {
      name: wpname,
      authorId: this.authenticationService.currentUserValue.id,
      authorEmail: this.authenticationService.currentUserValue.userName,
      authorName: this.authenticationService.currentUserValue.firstName + this.authenticationService.currentUserValue.lastName
    };

    return this.httpClient.post<WorkPlace>(`${environment.wpsApi}api/v1/WorkPlaces`, requestModel)
      .pipe(map(result => {
        this.workplace = result;
        return result;
      }));
  }
}
