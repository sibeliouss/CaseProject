import { Component } from '@angular/core';
import { TaskModel } from '../../models/task';
import { TasksStatus } from '../../models/taskStatusEnum';
import { AuthService } from '../../../core/services/auth.service';
import { HttpService } from '../../services/http.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { DropdownModule } from 'primeng/dropdown';
import { TaskUpdateModel } from '../../models/taskUpdate';
import { InputText, InputTextModule } from 'primeng/inputtext';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-detail-task',
  standalone: true,
  imports: [CommonModule, FormsModule, ButtonModule, DialogModule, DropdownModule, InputTextModule, InputTextareaModule],
  templateUrl: './detail-task.component.html',
  styleUrls: ['./detail-task.component.scss']
})
export class DetailTaskComponent {

  taskId: string = "";
  task: TaskModel = {} as TaskModel;
  visibleUpdateDialog: boolean = false;  // Güncelleme formu için
  visibleGalleryDialog: boolean = false; // Görseller için
  statusOptions = [
    { name: 'Yeni', value: TasksStatus.New },
    { name: 'Devam Ediyor', value: TasksStatus.InProgress },
    { name: 'Tamamlandı', value: TasksStatus.Completed }
  ];

  constructor(
    public auth: AuthService,
    private http: HttpService,
    private activate: ActivatedRoute,
    private router: Router,
    private messageService: MessageService
  ) {
    this.activate.params.subscribe((params) => {
      this.taskId = params['id'];
      this.getTask();
    });
  }

  showUpdateDialog() {
    this.visibleUpdateDialog = true;
  }

  showGalleryDialog() {
    this.visibleGalleryDialog = true;
  }

  getTask() {
    if (!this.taskId) {
      console.error("taskId, getTask için gerekli");
      return;
    }

    this.http.get(`Tasks/GetById?taskId=${this.taskId}`, (res) => {
      this.task = res;
    });
  }

  updateTask() {
    if (!this.taskId) {
      console.error("tskId, güncelleme için gerekli");
      return;
    }

    const updateDto: TaskUpdateModel = {
      Id: this.task.id,
      Title: this.task.title,
      Description: this.task.description,
      Status: this.task.status
    };

    this.http.update(`Tasks/Update`, updateDto, () => {
      this.messageService.add({ severity: 'info', summary: 'Başarılı bir şekilde güncellendi.', detail: '' });
      this.getTask();  // Güncelleme işleminden sonra güncellenmiş verileri almak için
    });
  }

  goToHome() {
    this.router.navigate(['/home']);
  }
}
