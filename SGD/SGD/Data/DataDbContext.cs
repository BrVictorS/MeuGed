using SGD.Models;
using Microsoft.EntityFrameworkCore;
using SGD.Models;

namespace SGD.Data
{
    public class DataDbContext: DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {            
        }
        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<DocumentoModel> Documentos { get; set; }
        public DbSet<LoteModel> Lote { get; set; }
        public DbSet<FluxoModel> FluxoLote { get; set; }
        public DbSet<MetadadosModel> Metadados { get; set;}
        public DbSet<ProjetoModel> Projeto { get; set; }
        public DbSet<ProtocoloModel> Protocolos { get; set; }
        public DbSet<TipoDocumentalModel> TipoDocumental { get; set;}        
        public DbSet<PermissoesModel> Permissoes { get; set; }
        public DbSet<UsuarioPermissaoModel> UsuarioPermissoes { get; set; }
        public DbSet<UsuarioProjetoModel> UsuarioProjetos { get; set; }
        public DbSet<SituacaoModel> Situacoes { get; set; }
        public DbSet<ParametrosModel> Parametros { get; set; }
        public DbSet<MetadadosTipoDocModel> MetadadosTipoDoc { get; set; }
        public DbSet<FuncaoModel> Funcao { get; set; }
        public DbSet<SubfuncaoModel> Subfuncao { get; set; }
        public DbSet<AtividadeModel> Atividade { get; set; }
        public DbSet<EspecieModel> Especie { get; set; }
        public DbSet<IndexacaoDocumentoModel> Indexacao { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IndexacaoDocumentoModel>()
                .HasKey(id => new { id.MetadadoTipoDocId, id.DocumentoId, id.LoteId }); // Chave composta


            // Configurando a tabela de junção explícita
            modelBuilder.Entity<UsuarioPermissaoModel>()
                .HasKey(up => new { up.UsuarioId, up.PermissaoId }); // Chave composta

            // Configurando relacionamento entre UsuarioPermissao e Usuario
            modelBuilder.Entity<UsuarioPermissaoModel>()
                .HasOne(up => up.Usuario) // Relacionamento 1:N (UsuarioPermissao -> Usuario)
                .WithMany(u => u.UsuarioPermissoes) // Relacionamento inverso (Usuario -> UsuarioPermissao)
                .HasForeignKey(up => up.UsuarioId); // Chave estrangeira

            // Configurando relacionamento entre UsuarioPermissao e Permissao
            modelBuilder.Entity<UsuarioPermissaoModel>()
                .HasOne(up => up.Permissao) // Relacionamento 1:N (UsuarioPermissao -> Permissao)
                .WithMany(p => p.UsuariosPermissoes) // Relacionamento inverso (Permissao -> UsuarioPermissao)
                .HasForeignKey(up => up.PermissaoId); // Chave estrangeira

            ///////
            
            // Configurando a tabela de junção explícita
            modelBuilder.Entity<ProjetoPermissaoModel>()
                .HasKey(up => new { up.ProjetoId, up.PermissaoId }); // Chave composta

            // Configurando relacionamento entre UsuarioPermissao e Usuario
            modelBuilder.Entity<ProjetoPermissaoModel>()
                .HasOne(up => up.Projeto) // Relacionamento 1:N (UsuarioPermissao -> Usuario)
                .WithMany(u => u.Permissoes) // Relacionamento inverso (Usuario -> UsuarioPermissao)
                .HasForeignKey(up => up.ProjetoId); // Chave estrangeira

            // Configurando relacionamento entre UsuarioPermissao e Permissao
            modelBuilder.Entity<ProjetoPermissaoModel>()
                .HasOne(up => up.Permissao) // Relacionamento 1:N (UsuarioPermissao -> Permissao)
                .WithMany(p => p.ProjetoPermissoes) // Relacionamento inverso (Permissao -> UsuarioPermissao)
                .HasForeignKey(up => up.ProjetoId); // Chave estrangeira

            /////
            ///

            // Configurando a tabela de junção explícita
            modelBuilder.Entity<UsuarioProjetoModel>()
                .HasKey(up => new { up.ProjetoId, up.UsuarioId }); // Chave composta

            // Configurando relacionamento entre UsuarioPermissao e Usuario
            modelBuilder.Entity<UsuarioProjetoModel>()
                .HasOne(up => up.Usuario) // Relacionamento 1:N (UsuarioPermissao -> Usuario)
                .WithMany(u => u.UsuariosProjetos) // Relacionamento inverso (Usuario -> UsuarioPermissao)
                .HasForeignKey(up => up.UsuarioId); // Chave estrangeira

            // Configurando relacionamento entre UsuarioPermissao e Permissao
            modelBuilder.Entity<UsuarioProjetoModel>()
                .HasOne(up => up.Projeto) // Relacionamento 1:N (UsuarioPermissao -> Permissao)
                .WithMany(p => p.UsuariosProjetos) // Relacionamento inverso (Permissao -> UsuarioPermissao)
                .HasForeignKey(up => up.ProjetoId); // Chave estrangeira


            base.OnModelCreating(modelBuilder);
        }
    }
}
