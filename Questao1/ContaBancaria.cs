using System;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace Questao1
{
    public class ContaBancaria
    {
        public int numero { get; private set; }
        public string titular { get; set; }
        public double depositoInicial { get; private set; }

        public ContaBancaria(int numero, string titular, double depositoInicial)
        {
            this.numero = numero;
            this.titular = titular;
            this.depositoInicial = depositoInicial;
        }

        public ContaBancaria(int numero, string titular)
        {
            this.numero = numero;
            this.titular = titular;
            this.depositoInicial = 0;
        }

        public void Deposito(double valor)
        {
            this.depositoInicial += valor;
        }

        public void Saque(double valor)
        {
            this.depositoInicial -= (valor + 3.50);
        }

        public override string ToString() => $"Conta {this.numero}, Titular: {this.titular}, Saldo: $ {this.depositoInicial}";



    }
}
