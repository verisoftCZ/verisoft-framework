import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { BaseHttpService, BASE_URL_PATH, Page, RequestParams, requestParamsToHttpParams } from "@verisoft/core";
import { Observable } from "rxjs";
import { Contract } from "../models/contract";

@Injectable({ providedIn: 'root' })
export class ContractService extends BaseHttpService<Contract> {
  constructor(
    override readonly http: HttpClient,
    @Inject(BASE_URL_PATH) basePath: string
  ) {
    super(http, basePath);
  }

  getContracts(requestParams: RequestParams<Contract>): Observable<Page<Contract>> {
    const params = requestParamsToHttpParams<Contract>(requestParams);
    return this.http.get<Page<Contract>>("https://hackaton.dev.verisoft.cz/api/" + 'v2/Contract/list', { params });
  }

}