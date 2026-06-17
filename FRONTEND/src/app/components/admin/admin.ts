import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AdminService } from '../../services/admin.service';
import { User } from '../../models/user.model';
import { Property } from '../../models/property.model';
import { UserRole } from '../../models/user-role.enum';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin.html',
  styleUrl: './admin.scss'
})
export class AdminComponent implements OnInit {
  users: User[] = [];
  properties: Property[] = [];

  newUser: User = { username: '', password: '' };
  selectedUserId: number | null = null;
  selectedPropertyId: number | null = null;

  message = '';

  constructor(private adminService: AdminService, private router: Router, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.loadUsers();
    this.loadProperties();
  }

  loadUsers() {
    this.adminService.getUsers().subscribe(u => {
      this.users = u.filter(user => user.userRole !== UserRole.Admin);
      this.cdr.detectChanges();
    });
  }

  loadProperties() {
    this.adminService.getProperties().subscribe(p => { this.properties = p; this.cdr.detectChanges(); });
  }

  addUser() {
    if (!this.newUser.username || !this.newUser.password) return;
    this.adminService.addUser(this.newUser).subscribe({
      next: () => {
        this.message = `המשתמש "${this.newUser.username}" נוצר בהצלחה`;
        this.newUser = { username: '', password: '' };
        this.loadUsers();
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.message = err.error?.message ?? 'שגיאה ביצירת המשתמש';
        this.cdr.detectChanges();
      }
    });
  }

  assignProperty() {
    if (this.selectedUserId == null || this.selectedPropertyId == null) return;
    this.adminService.assignProperty(this.selectedUserId, this.selectedPropertyId).subscribe(() => {
      this.message = 'הנכס שויך בהצלחה';
      this.selectedUserId = null;
      this.selectedPropertyId = null;
      this.loadUsers();
      this.cdr.detectChanges();
    });
  }

  removeProperty(user: User, property: Property) {
    if (!user.id || !property.id) return;
    this.adminService.unassignProperty(user.id, property.id).subscribe(() => {
      this.message = `הנכס "${property.name}" הוסר מ${user.username}`;
      this.loadUsers();
      this.cdr.detectChanges();
    });
  }

  goBack() {
    this.router.navigate(['/home']);
  }
}
