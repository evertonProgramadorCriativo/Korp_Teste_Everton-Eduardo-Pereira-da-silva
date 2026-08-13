import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subject, finalize, takeUntil } from 'rxjs';

import { ProdutoService } from '../../core/services/produto.service';
import { Produto } from '../../core/models/produto.model';

@Component({
  selector: 'app-produtos',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './produtos.component.html',
  styleUrl: './produtos.component.css'
})
export class ProdutosComponent implements OnInit, OnDestroy {
  produtos: Produto[] = [];
  carregando = false;
  enviando = false;
  erro: string | null = null;

  private readonly destroy$ = new Subject<void>();
  readonly form;

  constructor(
    private readonly produtoService: ProdutoService,
    private readonly fb: FormBuilder
  ) {
    this.form = this.fb.nonNullable.group({
      codigo: ['', [Validators.required, Validators.maxLength(50)]],
      descricao: ['', [Validators.required, Validators.maxLength(200)]],
      saldo: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit(): void {
    this.carregarProdutos();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  carregarProdutos(): void {
    this.carregando = true;
    this.erro = null;

    this.produtoService
      .listar()
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => (this.carregando = false))
      )
      .subscribe({
        next: (produtos) => (this.produtos = produtos),
        error: (erro: Error) => (this.erro = erro.message)
      });
  }

  cadastrar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.enviando = true;
    this.erro = null;

    this.produtoService
      .criar(this.form.getRawValue())
      .pipe(
        takeUntil(this.destroy$),
        finalize(() => (this.enviando = false))
      )
      .subscribe({
        next: () => {
          this.form.reset({ codigo: '', descricao: '', saldo: 0 });
          this.carregarProdutos();
        },
        error: (erro: Error) => (this.erro = erro.message)
      });
  }

  saldoBaixo(produto: Produto): boolean {
    return produto.saldo <= 5;
  }
}
