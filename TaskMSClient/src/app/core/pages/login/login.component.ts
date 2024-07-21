import { Component } from '@angular/core';
import { BaseInputErrorsComponent } from '../../components/base-input-errors/base-input-errors.component';
import { PasswordModule } from 'primeng/password';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastModule } from 'primeng/toast';
import { CheckboxModule } from 'primeng/checkbox';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [PasswordModule, FormsModule, ToastModule,CheckboxModule, ButtonModule, InputTextModule,ReactiveFormsModule,BaseInputErrorsComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  loginForm: FormGroup;

  constructor(
    private form: FormBuilder,
    private message: MessageService,
    private loginService: LoginService,
    private router: Router,
    private authService: AuthService
  ) {
    this.loginForm = this.form.group({
      userNameOrEmail: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [true],
    });
  }

  ngOnInit(): void {
    if (this.authService.checkAuthentication()) {
      this.router.navigateByUrl('/');
    }
  }

  signIn() {
    if (this.loginForm.invalid) {
      this.message.add({
        severity: 'warn',
        summary: 'Doğrulama Hatası',
        detail: 'Formu doğru doldurduğunuzdan emin olun.',
      });
      return;
    }

    const request = this.loginForm.value;

    this.loginService.post('Auth/Login', request).subscribe({
      next: (res: any) => {
        localStorage.setItem('response', JSON.stringify(res));
        this.authService.checkAuthentication();
        this.router.navigateByUrl('/home');
        this.message.add({
          severity: 'success',
          summary: 'Giriş',
          detail: 'Giriş Başarılı'
        });
       
      },
      error: (err: any) => {
        console.error("POST request failed", err);
        this.message.add({
          severity: 'error',
          summary: 'Giriş Hatası',
          detail: 'Giriş işlemi sırasında bir hata oluştu.'
        });
      }
    });
  }


}
