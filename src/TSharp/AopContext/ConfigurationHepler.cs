using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using TSharp.Core.Pattern;

namespace TSharp.Core.Util
{
  using Microsoft.Extensions.Configuration;

  /// <summary>
    /// Configuration Hepler Class
    /// </summary>
    /// <author>
    /// tangjingbo
    /// </author>
    /// <remarks>
    /// tangj15 at 2012-3-15 17:23
    /// </remarks>
    public static class ConfigurationHepler
    {
        /// <summary>
        /// 对Configuration进行修改，然后保存
        /// </summary>
        /// <returns></returns>
        public static Configuration GetConfigConfiguration()
        { 
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return config;
        }

        /// <summary>
        /// Gets the config configuration.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static Configuration GetConfigConfiguration(string file)
        {
            var fileMap = new ExeConfigurationFileMap
                              {
                                  ExeConfigFilename = file
                              };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            return config;
        }

        /// <summary>
        /// Saves as.
        /// </summary>
        /// <param name="config">The config.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static Configuration SaveAsTSharpFile(this Configuration config, string path)
        {
            if (string.IsNullOrEmpty(path))
                path = string.Format(@"{0}.temp.config", config.FilePath);
            else if (string.IsNullOrEmpty(Path.GetPathRoot(path)))
                path = string.Format(@"{0}.{1}.config", config.FilePath, path);
            config.SaveAs(path, ConfigurationSaveMode.Full);
            return config;
        }

        /// <summary>
        /// 对配置节分组进行修改，然后保存
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="func">The action.</param>
        /// <param name="sectionGroupName">Name of the section group.</param>
        /// <returns></returns>
        public static Configuration UpdateSectionGroup(this Configuration x, Action<ConfigurationSectionGroup> func,
                                                       string sectionGroupName)
        {
            ConfigurationSectionGroup group = x.SectionGroups.Get(sectionGroupName);
            if (group == null)
            {
                group = new ConfigurationSectionGroup();
                x.SectionGroups.Add(sectionGroupName, group);
                func(group);
            }
            else
            {
                func(group);
            }
            return x;
        }

        /// <summary>
        /// 在configuration上直接保存一个Section
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x">The x.</param>
        /// <param name="updater">The updater.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        public static Configuration UpdateSection<T>(this Configuration x, Func<T, T> updater, string sectionName)
            where T : ConfigurationSection
        {
            var section = x.Sections.Get(sectionName) as T;
            if (section != null)
            {
                section = updater(section);
            }
            else
            {
                section = updater(section);
                //x.Sections.Remove(sectionName);
                x.Sections.Add(sectionName, section);
            }

            return x;
        }


        /// <summary>
        /// 在名称为<paramref name="sectionGroupName"/>的配置节分组上保存一个Section
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config">The x.</param>
        /// <param name="updater">The updater.</param>
        /// <param name="sectionGroupName">Name of the section group.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <returns></returns>
        public static Configuration UpdateSection<T>(this Configuration config, Func<T, T> updater,
                                                     string sectionGroupName, string sectionName)
            where T : ConfigurationSection
        {
            return UpdateSectionGroup(config, group =>
                                                  {
                                                      var section = group.Sections.Get(sectionName) as T;
                                                      if (section == null)
                                                      {
                                                          try
                                                          {
                                                              section = Activator.CreateInstance<T>();
                                                          }
                                                          catch (Exception)
                                                          {
                                                          }
                                                          section = updater(section);
                                                          group.Sections.Add(sectionName, section);
                                                      }
                                                      else
                                                      {
                                                          updater(section);
                                                      }

                                                  }, sectionGroupName);
        }

        /// <summary>
        /// Gets the or add.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="name">The name.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns></returns>
        public static T GetOrAdd<T>(this ConfigurationSectionCollection obj, string name, Func<string, T> valueFactory)
            where T : ConfigurationSection
        {
            var temp = obj.Get(name) as T;
            if (temp != null)
                return temp;
            obj.Add(name, temp = valueFactory(name));
            return temp;
        }

        /// <summary>
        /// Adds the or update.
        /// update时无法取到原有配置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="name">The name.</param>
        /// <param name="addFactory">The add factory.</param>
        /// <param name="updateFactory">The update factory.</param>
        /// <returns></returns>
        public static T AddOrUpdate<T>(this ConfigurationSectionCollection obj, string name, Func<string, T> addFactory,
                                       Func<string, T, T> updateFactory) where T : ConfigurationSection
        {
            ConfigurationSection t = obj.Get(name);
            if (t == null)
            {
                T addValue = addFactory(name);
                obj.Add(name, addValue);
                return addValue;
            }
            else
            {
                var temp = (T)t;
                obj.Remove(name);
                obj.Add(name, temp = updateFactory(name, temp));
                return temp;
            }
        }

        /// <summary>
        /// 获取配置接AppSetting中的值，如果没有配置则返回默认值
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        private static string GetAppSettingValue(string key, string defaultValue)
        {
            string v = ConfigurationManager.AppSettings.Get(key);
            if (v == null)
                return defaultValue;
            return v;
        }


        /// <summary>
        /// 获取配置接AppSetting中的值，如果没有配置则返回默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">配置节的Key.</param>
        /// <param name="valueFactory">配置对象创建工厂。传入参数为配置中的字符串</param>
        /// <returns></returns>
        private static T GetAppSettingValue<T>(string key, Func<string, T> valueFactory)
        {
            return ConfigKeyCache<T>.Current.GetOrAdd(key, k => valueFactory(ConfigurationManager.AppSettings.Get(k)));
        }

        /// <summary>
        /// 以当前字符串为key,获取配置接AppSetting中的值，如果没有配置则返回默认值
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetAppSetting(this string key, string defaultValue)
        {
            return GetAppSettingValue(key, defaultValue);
        }

        /// <summary>
        /// 以当前字符串为key,获取配置接AppSetting中的值，如果没有配置则返回默认值
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetAppSetting(this string key)
        {
            return GetAppSettingValue(key, default(string));
        }

        /// <summary>
        /// 以当前字符串为key,获取配置字符串，并通过valueFactory转换为指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="valueFactory">The value factory.</param>
        /// <returns></returns>
        public static T GetAppSetting<T>(this string key, Func<string, T> valueFactory)
        {
            return GetAppSettingValue(key, valueFactory);
        }

        #region Nested type: ConfigKeyCache

        private class ConfigKeyCache<T> : ConcurrentDictionary<string, T>
        {
            private ConfigKeyCache()
            {
            }

            public static ConfigKeyCache<T> Current
            {
                get
                {
                    return
                        SingletonHelper<ConfigKeyCache<T>>.GetOrAdd(
                            () => new ConfigKeyCache<T>());
                }
            }
        }

        #endregion
    }
}