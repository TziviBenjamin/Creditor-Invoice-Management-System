import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { InvoiceService } from '../../services/invoice.service';
import { Invoice } from '../../models/invoice.model';
import { Property } from '../../models/property.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.scss'
})
export class HomeComponent implements OnInit {
  properties: Property[] = [];
  invoices: Invoice[] = [];
  username: string | null = null;
  isAdmin = false;
  
  loading = false;
  error = '';
  searched = false;
  
  // שימוש באובייקט filters לניהול מצב החיפוש
  filters = { 
    propertyId: undefined as number | undefined, 
    supplierName: '', 
    fromDate: '', 
    toDate: '' 
  };

  constructor(
    private invoiceService: InvoiceService, 
    private authService: AuthService,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.invoiceService.getMyProperties().subscribe({
      next: (props) => { this.properties = props; this.cdr.detectChanges(); },
      error: () => { this.error = 'שגיאה בטעינת הנכסים'; this.cdr.detectChanges(); }
    });
    
    this.username = this.authService.getUsername();
    this.isAdmin = this.authService.isAdmin();
  }

  search() {
    this.error = '';
    this.loading = true;
    this.searched = true;
    
    const { propertyId, supplierName, fromDate, toDate } = this.filters;
    
    this.invoiceService.searchInvoices(
      propertyId, 
      supplierName || undefined, 
      fromDate || undefined, 
      toDate || undefined
    ).subscribe({
      next: (results) => {
        this.invoices = results;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'שגיאה בחיפוש';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  openPdf(invoice: Invoice) {
    if (!invoice.id) return;
    this.invoiceService.getPdfBlob(invoice.id).subscribe({
      next: (blob) => {
        const url = URL.createObjectURL(blob);
        window.open(url, '_blank', 'noopener');
      },
      error: () => {
        // fallback לנתיב ישיר אם קיים
        if (invoice.filePath) {
          const base = 'https://localhost:7144';
          const url = /^https?:\/\//i.test(invoice.filePath)
            ? invoice.filePath
            : `${base}/${invoice.filePath.replace(/^\//, '')}`;
          window.open(url, '_blank', 'noopener');
        } else {
          this.error = 'לא ניתן לפתוח את הקובץ';
        }
      }
    });
  }

  goToAdmin() {
    this.router.navigate(['/admin']);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}