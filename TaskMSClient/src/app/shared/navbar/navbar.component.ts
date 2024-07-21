import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { MenuItem, MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { MenubarModule } from 'primeng/menubar';
import { ToastModule } from 'primeng/toast';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [MenubarModule, RouterOutlet,InputTextModule, ButtonModule,ToastModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent {

  items: MenuItem[] | undefined;

  constructor(
    private router: Router,
    public auth: AuthService,
    private message: MessageService 
  ){}

  ngOnInit() {
    this.auth.checkAuthentication();
    this.items = [
      {
        label: 'Ana Sayfa',
        icon: 'pi pi-fw pi-home',
        routerLink: "/home"
      },
    ];
  }

  logout(){
    localStorage.removeItem("response");
    this.auth.token = null;
   
    this.message.add({severity: 'success',
    detail: 'Çıkış Başarılı'});
    //1 sn sonra login sayfasını getir. 
    setTimeout(() => {
      this.router.navigateByUrl("/login");
    }, 1000);

  }

}
