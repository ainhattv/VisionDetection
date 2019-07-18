import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { WorkPlace } from '../../shared/models/workplace.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WorkplaceService {
  public workplace: Observable<WorkPlace>;

  constructor(private httpClient: HttpClient) {

  }

  public loadWorkPlace(userid: string) {
    return this.httpClient.get<Observable<WorkPlace>>(`${environment.authApi}api/v1//api/v1/WorkPlaces/${userid}`)
      .pipe(map(wp => {
        this.workplace = wp;
      }));
  }
}
