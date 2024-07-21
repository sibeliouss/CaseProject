import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BreadcrumbModule } from 'primeng/breadcrumb';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { DialogService, DynamicDialogModule, DynamicDialogRef } from 'primeng/dynamicdialog';
import { InputTextModule } from 'primeng/inputtext';
import { TableModule } from 'primeng/table';
import { TagModule } from 'primeng/tag';
import { TaskModel } from '../../models/task';
import { MessageService } from 'primeng/api';
import { HttpService } from '../../services/http.service';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

import { CreateTaskComponent } from '../create-task/create-task.component';
import { TasksStatus } from '../../models/taskStatusEnum';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, BreadcrumbModule, TableModule, TagModule, InputTextModule, ButtonModule, DialogModule, DynamicDialogModule, DropdownModule, FormsModule],
  providers: [DialogService],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {

  tasks: TaskModel[] = [];
  ref: DynamicDialogRef | undefined;


  colDefs = [
    { field: 'userName', header: 'Kullanıcı Adı' },
    { field: 'title', header: 'Başlık' },
    { field: 'description', header: 'Açıklama' },
    { field: 'createAt', header: 'Oluşturulma Tarihi' },
    { field: 'status', header: 'Durum' },
    { field: 'delete', header: 'Sil' }
  ];

  statusOptions = [
    { label: 'Tüm Durumlar', value: 'all' },
    { label: 'Yeni', value: 'New' },
    { label: 'Devam Edenler', value: 'InProgress' },
    { label: 'Tamamlananlar', value: 'Completed' }
  ];

  selectedStatus: string = 'all';

  constructor(
    public dialogService: DialogService,
    public messageService: MessageService,
    private http: HttpService,
    private router: Router,
    private auth: AuthService
  ) { }

 
  ngOnInit(): void {
    this.getAll();
  }

  goToDetail(id: string) {
    this.router.navigateByUrl("/task-details/" + id);
  }

  getAll() {
    const data = {
      roles: this.auth.token?.roles
    };

    this.http.post("Tasks/GetAll", data, (res) => {
      this.tasks = res.map((r: any) => ({
        id: r.id,
        title: r.title,
        description: r.description,
        userName: r.userName,
        status: TasksStatus[r.status as keyof typeof TasksStatus] || TasksStatus.New,
        createAt: new Date(r.createAt),
        fileUrls: r.fileUrls || []
      }));


      // Yeni görevler önce
     // this.tasks.sort((a, b) => b.createAt.getTime() - a.createAt.getTime());
      this.tasks = [...this.tasks];

    });
  }

  show() {
    this.ref = this.dialogService.open(CreateTaskComponent, {
      header: 'Görev Ekle',
      width: '30%',
      contentStyle: { overflow: 'auto' },
      baseZIndex: 10000,
      maximizable: false,
    });

    this.ref.onClose.subscribe((data: any) => {
      if (data) {
        data.status = TasksStatus.New;
        this.http.post("Tasks/Add", data, (res) => {
          this.getAll();
          this.messageService.add({ severity: 'success', summary: 'Yeni görev oluşturuldu.', detail: '' });
        });
      }
    });

    this.ref.onMaximize.subscribe((value) => {
      this.messageService.add({ severity: 'info', summary: 'Maximized', detail: `maximized: ${value.maximized}` });
    });
  }

  deleteTask(taskId: string) {
    this.http.delete(`Tasks/Delete/${taskId}`, (res) => {
      this.getAll();
      this.messageService.add({ severity: 'success', summary: 'Görev Silindi', detail: '' });
    });
  }

  getSeverity(durum: string) {
    switch (durum) {
        case 'New':
            return 'success';
        case 'InProgress':
            return 'warning';
        case 'Completed':
            return 'danger';
        default:
          return;
    }
}

getStatusLabel(status: string): string {
  switch (status) {
    case 'New':
      return 'Yeni';
    case 'InProgress':
      return 'Devam Ediyor';
    case 'Completed':
      return 'Tamamlandı';
    default:
      return status;
  }
}

filterTasksByStatus(status: string) {
 
  if (status === 'all') {
    this.getAll(); // Reset to all tasks
  } else {
    this.http.get(`Tasks/FilterByStatus?status=${status}`, (res) => {
      this.tasks = res.map((r: any) => ({
        id: r.id,
        title: r.title,
        description: r.description,
        userName: r.userName,
        status: TasksStatus[r.status as keyof typeof TasksStatus] || TasksStatus.New,
        createAt: new Date(r.createAt),
        fileUrls: r.fileUrls || []
      }));

    });
  }
}

}
