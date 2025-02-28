import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { BaseHttpService, BASE_URL_PATH } from "@verisoft/core";
import { Document } from "../models/document";

@Injectable({
  providedIn: 'root'
})

@Injectable({ providedIn: 'root'})
export class DocumentService extends BaseHttpService<Document> {
  constructor(
  override readonly http: HttpClient,
  @Inject(BASE_URL_PATH) basePath: string
  ) {
  super(http, basePath);
  }
}
