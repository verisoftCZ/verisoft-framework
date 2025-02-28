import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {
  BaseHttpService,
  BASE_URL_PATH,
  Page,
  RequestParams,
  requestParamsToHttpParams,
} from '@verisoft/core';
import { map, Observable } from 'rxjs';
import { Client } from '../models/client.model';

@Injectable({
  providedIn: 'root',
})
export class V2EndpointService extends BaseHttpService<any> {
  constructor(
    override readonly http: HttpClient,
    @Inject(BASE_URL_PATH) basePath: string
  ) {
    super(http, basePath);
    this.entityName = 'v2/Client';
  }

  getXD(req: RequestParams<any>) {
    return this.fetchList(req);
  }


  getClients(req: RequestParams<Client>): Observable<Page<Client>> {
    const params = requestParamsToHttpParams<Client>(req);
    return this.http
      .get<Page<Client>>(this.basePath + 'v2/Client/list', { params })
      .pipe(
        map((request) => {
          request.size = req.size;
          return request;
        })
      );
  }
}
