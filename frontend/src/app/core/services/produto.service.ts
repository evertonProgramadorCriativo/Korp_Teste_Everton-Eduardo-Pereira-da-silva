import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';

import { environment } from '../../../environments/environment';
import { Produto, ProdutoCreate } from '../models/produto.model';

@Injectable({ providedIn: 'root' })
export class ProdutoService {
  private readonly baseUrl = `${environment.estoqueApiUrl}/produtos`;

  constructor(private readonly http: HttpClient) {}

  listar(saldoMinimo?: number): Observable<Produto[]> {
    const url =
      saldoMinimo != null
        ? `${this.baseUrl}?saldoMinimo=${saldoMinimo}`
        : this.baseUrl;

    return this.http.get<Produto[]>(url).pipe(catchError(this.tratarErro));
  }

  criar(produto: ProdutoCreate): Observable<Produto> {
    return this.http
      .post<Produto>(this.baseUrl, produto)
      .pipe(catchError(this.tratarErro));
  }

  private tratarErro(erro: HttpErrorResponse) {
    let mensagem = 'Não foi possível completar a operação. Tente novamente.';

    if (erro.status === 409) {
      mensagem =
        erro.error?.mensagem ?? 'Já existe um produto com esse código.';
    } else if (erro.status === 400) {
      mensagem = 'Verifique os campos preenchidos e tente novamente.';
    } else if (erro.status === 0) {
      mensagem = 'Serviço de Estoque indisponível no momento.';
    }

    return throwError(() => new Error(mensagem));
  }
}
