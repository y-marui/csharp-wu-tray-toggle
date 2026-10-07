namespace WuTrayToggle;

// zh/hi/es/fr/pt strings are machine-translated (no native review) — flag any
// wording issues found in use as an issue rather than silently hand-editing.
internal static class Strings
{
    public static string MenuCheckStatus => L(
        "現在の状態を確認",
        "Check current status",
        "查看当前状态",
        "वर्तमान स्थिति जांचें",
        "Comprobar estado actual",
        "Vérifier l'état actuel",
        "Verificar estado atual");

    public static string MenuStartup => L(
        "ログイン時に自動起動",
        "Start at login",
        "开机时自动启动",
        "लॉगिन पर स्वतः प्रारंभ करें",
        "Iniciar con el sistema",
        "Démarrer avec la connexion",
        "Iniciar com o login");

    public static string MenuSettings => L(
        "設定…",
        "Settings…",
        "设置…",
        "सेटिंग्स…",
        "Configuración…",
        "Paramètres…",
        "Configurações…");

    public static string SettingsTitle => L(
        "設定",
        "Settings",
        "设置",
        "सेटिंग्स",
        "Configuración",
        "Paramètres",
        "Configurações");

    public static string ButtonSave => L("保存", "Save", "保存", "सहेजें", "Guardar", "Enregistrer", "Salvar");

    public static string ButtonCancel => L(
        "キャンセル",
        "Cancel",
        "取消",
        "रद्द करें",
        "Cancelar",
        "Annuler",
        "Cancelar");

    public static string MenuLanguage => L("言語", "Language", "语言", "भाषा", "Idioma", "Langue", "Idioma");

    public static string MenuLanguageSystem => L(
        "システム既定",
        "System default",
        "系统默认",
        "सिस्टम डिफ़ॉल्ट",
        "Predeterminado del sistema",
        "Paramètre système",
        "Padrão do sistema");

    public static string LanguageNameJapanese => "日本語";

    public static string LanguageNameEnglish => "English";

    public static string LanguageNameChinese => "中文";

    public static string LanguageNameHindi => "हिन्दी";

    public static string LanguageNameSpanish => "Español";

    public static string LanguageNameFrench => "Français";

    public static string LanguageNamePortuguese => "Português";

    public static string MenuStop => L(
        "停止 (制御開始)",
        "Stop (enable control)",
        "停止（开启控制）",
        "रोकें (नियंत्रण सक्षम करें)",
        "Detener (activar control)",
        "Arrêter (activer le contrôle)",
        "Parar (ativar controle)");

    public static string MenuStart => L(
        "再開 (通常)",
        "Resume (normal)",
        "恢复（正常）",
        "फिर से शुरू करें (सामान्य)",
        "Reanudar (normal)",
        "Reprendre (normal)",
        "Retomar (normal)");

    public static string MenuExit => L("終了", "Exit", "退出", "बाहर निकलें", "Salir", "Quitter", "Sair");

    public static string TrayTitle => L(
        "WU トレイ",
        "WU Tray",
        "WU 托盘",
        "WU ट्रे",
        "Bandeja WU",
        "Barre d'état WU",
        "Bandeja WU");

    public static string StatusTitle => L(
        "WU 状態確認",
        "WU Status",
        "WU 状态",
        "WU स्थिति",
        "Estado de WU",
        "État de WU",
        "Status do WU");

    public static string TrayTextRunning => L(
        "WU: 稼働中 (通常モード)",
        "WU: Running (normal)",
        "WU：运行中（正常模式）",
        "WU: चल रहा है (सामान्य मोड)",
        "WU: en ejecución (modo normal)",
        "WU : en cours d'exécution (mode normal)",
        "WU: em execução (modo normal)");

    public static string TrayTextStopped => L(
        "WU: 停止中 (制御モード)",
        "WU: Stopped (controlled)",
        "WU：已停止（受控模式）",
        "WU: रुका हुआ है (नियंत्रित मोड)",
        "WU: detenido (modo controlado)",
        "WU : arrêté (mode contrôlé)",
        "WU: parado (modo controlado)");

    public static string PolicyStopped => L(
        "1 (停止)",
        "1 (stopped)",
        "1（已停止）",
        "1 (रुका हुआ)",
        "1 (detenido)",
        "1 (arrêté)",
        "1 (parado)");

    public static string PolicyRunning => L(
        "0 (稼働中)",
        "0 (running)",
        "0（运行中）",
        "0 (चल रहा है)",
        "0 (en ejecución)",
        "0 (en cours d'exécution)",
        "0 (em execução)");

    public static string ServiceRunning => L(
        "実行中",
        "Running",
        "运行中",
        "चल रहा है",
        "En ejecución",
        "En cours d'exécution",
        "Em execução");

    public static string ServiceStopped => L(
        "停止中",
        "Stopped",
        "已停止",
        "रुका हुआ है",
        "Detenido",
        "Arrêté",
        "Parado");

    public static string ServiceUnknown => L(
        "サービス不明",
        "Service unknown",
        "服务状态未知",
        "सेवा स्थिति अज्ञात",
        "Estado del servicio desconocido",
        "État du service inconnu",
        "Status do serviço desconhecido");

    public static string StatusReportFormat => L(
        "【現在の詳細状態】\nアプリバージョン: {2}\nポリシー(NoAutoUpdate): {0}\nサービス状態: {1}",
        "Current status:\nApp version: {2}\nPolicy (NoAutoUpdate): {0}\nService status: {1}",
        "当前详细状态：\n应用版本：{2}\n策略 (NoAutoUpdate)：{0}\n服务状态：{1}",
        "वर्तमान विस्तृत स्थिति:\nऐप संस्करण: {2}\nनीति (NoAutoUpdate): {0}\nसेवा स्थिति: {1}",
        "Estado actual:\nVersión de la aplicación: {2}\nDirectiva (NoAutoUpdate): {0}\nEstado del servicio: {1}",
        "État actuel :\nVersion de l'application : {2}\nStratégie (NoAutoUpdate) : {0}\nÉtat du service : {1}",
        "Status atual:\nVersão do aplicativo: {2}\nPolítica (NoAutoUpdate): {0}\nStatus do serviço: {1}");

    public static string BalloonStopped => L(
        "Windows Update を停止しました",
        "Windows Update stopped.",
        "Windows Update 已停止。",
        "Windows Update रोक दिया गया है।",
        "Windows Update se ha detenido.",
        "Windows Update a été arrêté.",
        "O Windows Update foi parado.");

    public static string BalloonResumed => L(
        "Windows Update を再開しました",
        "Windows Update resumed.",
        "Windows Update 已恢复。",
        "Windows Update फिर से शुरू किया गया है।",
        "Windows Update se ha reanudado.",
        "Windows Update a été repris.",
        "O Windows Update foi retomado.");

    public static string BalloonCancelled => L(
        "操作をキャンセルしました",
        "Operation cancelled.",
        "操作已取消。",
        "कार्रवाई रद्द कर दी गई।",
        "Operación cancelada.",
        "Opération annulée.",
        "Operação cancelada.");

    public static string BalloonFailed => L(
        "操作に失敗しました",
        "Operation failed.",
        "操作失败。",
        "कार्रवाई विफल रही।",
        "Error en la operación.",
        "Échec de l'opération.",
        "Falha na operação.");

    public static string BalloonLanguageSaveFailed => L(
        "言語設定を保存できませんでした",
        "Could not save the language setting.",
        "无法保存语言设置。",
        "भाषा सेटिंग सहेजी नहीं जा सकी।",
        "No se pudo guardar la configuración de idioma.",
        "Impossible d'enregistrer le paramètre de langue.",
        "Não foi possível salvar a configuração de idioma.");

    public static string AlreadyRunning => L(
        "WU_TrayIcon はすでに起動しています。",
        "WU_TrayIcon is already running.",
        "WU_TrayIcon 已在运行。",
        "WU_TrayIcon पहले से ही चल रहा है।",
        "WU_TrayIcon ya se está ejecutando.",
        "WU_TrayIcon est déjà en cours d'exécution.",
        "WU_TrayIcon já está em execução.");

    private static string L(string ja, string en, string zh, string hi, string es, string fr, string pt)
    {
        return Localization.Current switch
        {
            AppLanguage.Japanese => ja,
            AppLanguage.Chinese => zh,
            AppLanguage.Hindi => hi,
            AppLanguage.Spanish => es,
            AppLanguage.French => fr,
            AppLanguage.Portuguese => pt,
            _ => en,
        };
    }
}
