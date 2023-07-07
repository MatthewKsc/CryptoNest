import { Component, OnInit, signal, WritableSignal } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { MatToolbarModule } from '@angular/material/toolbar';
import { RouterModule } from '@angular/router';
import { SystemInformationService } from '../../core/system-information/system-information.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, MatToolbarModule, NgOptimizedImage, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent {
  systemVersion: WritableSignal<string> = signal('');
  private readonly cryptoNestGithubUrl: string = 'https://github.com/MatthewKsc/CryptoNest';

  constructor(private systemInformationService: SystemInformationService) {
    this.systemVersion.set(systemInformationService.systemVersion)
  }

  navigateToProjectGithub() {
    window.open(this.cryptoNestGithubUrl, '_blank')
  }
}
