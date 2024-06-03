import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination } from '../shared/models/IPagination';
import { IProduct } from '../shared/models/IProduct';
import { IBrand } from '../shared/models/IBrand';
import { IType } from '../shared/models/IType';
import { StoreParams } from '../shared/models/StoreParams';

@Injectable({
  providedIn: 'root',
})
export class StoreService {
  constructor(private httpClient: HttpClient) {}

  baseUrl: string = 'http://localhost:8010';

  getProducts(storeParams: StoreParams) {
    let params = new HttpParams();
    if (storeParams.brandId)
      params = params.append('brandId', storeParams.brandId);

    if (storeParams.typeId)
      params = params.append('typeId', storeParams.typeId);

    if (storeParams.search)
      params = params.append('search', storeParams.search);

    params = params.append('pageIndex', storeParams.pageNumber);

    params = params.append('pageSize', storeParams.pageSize);

    params = params.append('sort', storeParams.sort);

    return this.httpClient.get<IPagination<IProduct>>(
      `${this.baseUrl}/Catalog/GetAllProducts`,
      { params }
    );
  }

  getBrands() {
    return this.httpClient.get<IBrand[]>(
      `${this.baseUrl}/Catalog/GetAllProductBrands`
    );
  }

  getProductTypes() {
    return this.httpClient.get<IType[]>(
      `${this.baseUrl}/Catalog/GetAllProductTypes`
    );
  }

  getProductById(id: string) {
    return this.httpClient.get<IProduct>(
      `${this.baseUrl}/Catalog/GetProductById/${id}`
    );
  }
}
