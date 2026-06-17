import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { Invoice } from '../models/invoice.model';
import { Property } from '../models/property.model';

@Injectable({ providedIn: 'root' })
export class InvoiceService {
  private api = 'https://localhost:7144/api';

  constructor(private http: HttpClient, private auth: AuthService) {}

  private headers(): HttpHeaders {
    return new HttpHeaders({ Authorization: `Bearer ${this.auth.getToken()}` });
  }

  getMyProperties(): Observable<Property[]> {
    return this.http.get<Property[]>(`${this.api}/invoice/my-properties`, { headers: this.headers() });
  }

  searchInvoices(propertyId?: number, supplierName?: string, fromDate?: string, toDate?: string): Observable<Invoice[]> {
    let params = new HttpParams();
    if (propertyId) params = params.set('propertyId', propertyId);
    if (supplierName) params = params.set('supplierName', supplierName);
    if (fromDate) params = params.set('fromDate', fromDate);
    if (toDate) params = params.set('toDate', toDate);
    return this.http.get<Invoice[]>(`${this.api}/invoice/search`, { headers: this.headers(), params });
  }

  getPdfUrl(invoiceId: number): string {
    return `${this.api}/invoice/${invoiceId}/pdf`;
  }

  getPdfBlob(invoiceId: number): Observable<Blob> {
    return this.http.get(`${this.api}/invoice/${invoiceId}/pdf`, {
      headers: this.headers(),
      responseType: 'blob'
    });
  }
}
