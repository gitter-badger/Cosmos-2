﻿using System;
using System.Collections.Generic;
using Cosmos.Logging.Configurations;
using Cosmos.Logging.Core;
using Cosmos.Logging.Events;

namespace Cosmos.Logging.Sinks.NLog {
    public class NLogSinkOptions : ILoggingSinkOptions<NLogSinkOptions>, ILoggingSinkOptions {
        public string Key => Internals.Constants.SinkKey;

        #region Append log minimum level

        internal readonly Dictionary<string, LogEventLevel> InternalNavigatorLogEventLevels = new Dictionary<string, LogEventLevel>();

        internal LogEventLevel? MinimumLevel { get; set; }

        public NLogSinkOptions UseMinimumLevelForType(Type type, LogEventLevel level) {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var typeName = TypeNameHelper.GetTypeDisplayName(type);
            if (InternalNavigatorLogEventLevels.ContainsKey(typeName)) {
                InternalNavigatorLogEventLevels[typeName] = level;
            } else {
                InternalNavigatorLogEventLevels.Add(typeName, level);
            }

            return this;
        }

        public NLogSinkOptions UseMinimumLevelForNamespace(Type type, LogEventLevel level) {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var @namespace = type.Namespace;
            return UseMinimumLevelForNamespace(@namespace, level);
        }

        public NLogSinkOptions UseMinimumLevelForNamespace(string @namespace, LogEventLevel level) {
            if (string.IsNullOrWhiteSpace(@namespace)) throw new ArgumentNullException(nameof(@namespace));
            @namespace = $"{@namespace}.*";
            if (InternalNavigatorLogEventLevels.ContainsKey(@namespace)) {
                InternalNavigatorLogEventLevels[@namespace] = level;
            } else {
                InternalNavigatorLogEventLevels.Add(@namespace, level);
            }

            return this;
        }

        public NLogSinkOptions UseMinimumLevel(LogEventLevel? level) {
            MinimumLevel = level;
            return this;
        }

        #endregion
        
        #region Append log level alias

        internal readonly Dictionary<string, LogEventLevel> InternalAliases = new Dictionary<string, LogEventLevel>();

        public NLogSinkOptions UseAlias(string alias, LogEventLevel level) {
            if (string.IsNullOrWhiteSpace(alias)) return this;
            if (InternalAliases.ContainsKey(alias)) {
                InternalAliases[alias] = level;
            } else {
                InternalAliases.Add(alias, level);
            }

            return this;
        }

        #endregion

        public global::NLog.Config.LoggingConfiguration OriginConfiguration { get; set; }

        public void UseDefaultOriginConfigFilePath() => OriginConfigFilePath = "nlog.config";
        public string OriginConfigFilePath { get; set; }

        public void EnableUsingDefaultConfig() => DoesUsedDefaultConfig = true;
        public void DisableUsingDefaultConfig() => DoesUsedDefaultConfig = false;
        internal bool DoesUsedDefaultConfig { get; set; } = true;
    }
}