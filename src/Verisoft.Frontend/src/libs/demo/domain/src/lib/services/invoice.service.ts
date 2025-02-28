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
import { Invoice } from '../models/invoice.model';

@Injectable({
  providedIn: 'root',
})
export class InvoiceService extends BaseHttpService<any> {
  constructor(
    override readonly http: HttpClient,
    @Inject(BASE_URL_PATH) basePath: string
  ) {
    super(http, basePath);
  }

  getInvoices(req: RequestParams<Invoice>): Observable<Page<Invoice>> {
    const params = requestParamsToHttpParams<Invoice>(req);
    return this.http
      .get<Page<Invoice>>(
        'https://hackaton.dev.verisoft.cz/api/' + 'v2/Client/list',
        { params }
      )
      .pipe(
        map((request) => {
          request.size = req.size;
          return request;
        })
      );
  }
}
