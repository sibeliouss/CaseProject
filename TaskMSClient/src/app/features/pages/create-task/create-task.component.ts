import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { FileUploadModule } from 'primeng/fileupload';
import { InputTextModule } from 'primeng/inputtext';
import { ToastModule } from 'primeng/toast';

interface UploadEvent {
  originalEvent: Event;
  files: File[];
}

@Component({
  selector: 'app-create-task',
  standalone: true,
  imports: [CommonModule, InputTextModule, FormsModule, FileUploadModule, ToastModule, InputTextModule],
  templateUrl: './create-task.component.html',
  styleUrl: './create-task.component.scss'
})
export class CreateTaskComponent {

  title: string = "";
  description: string = "";
  uploadedFiles: any[] = [];


  constructor(
    private messageService: MessageService,
    private dialog: DynamicDialogRef) { }

  onUpload(event: any) {
    for (let file of event.files) {
      this.uploadedFiles.push(file);
      console.log(this.uploadedFiles);
    }

    this.messageService.add({ severity: 'info', summary: 'Dosya yüklendi.', detail: '' });
  }

  create() {
    if (this.title === "") {
      this.messageService.add({ severity: 'error', summary: 'Başlık alanı boş olamaz', detail: 'Lütfen değer girin' });
      return;
    }
   

    if (this.description === "") {
      this.messageService.add({ severity: 'error', summary: 'Açıklama boş olamaz', detail: 'Lütfen değer girin' });
      return;
    }

    const formData = new FormData();
    formData.append("title", this.title);
    formData.append("description", this.description);
    for (let file of this.uploadedFiles) {
      formData.append("files", file, file.name);
    }
    //home componente göndermek için.
    this.dialog.close(formData);
  }

}
