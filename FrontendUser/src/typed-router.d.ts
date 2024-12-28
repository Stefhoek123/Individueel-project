/* eslint-disable */
/* prettier-ignore */
// @ts-nocheck
// Generated by unplugin-vue-router. ‼️ DO NOT MODIFY THIS FILE ‼️
// It's recommended to commit this file.
// Make sure to add this file to your tsconfig.json file as an "includes" or "files" entry.

declare module 'vue-router/auto-routes' {
  import type {
    RouteRecordInfo,
    ParamValue,
    ParamValueOneOrMore,
    ParamValueZeroOrMore,
    ParamValueZeroOrOne,
  } from 'vue-router'

  /**
   * Route name map generated by unplugin-vue-router
   */
  export interface RouteNamedMap {
    '/': RouteRecordInfo<'/', '/', Record<never, never>, Record<never, never>>,
    '/families/[id]': RouteRecordInfo<'/families/[id]', '/families/:id', { id: ParamValue<true> }, { id: ParamValue<false> }>,
    '/families/create': RouteRecordInfo<'/families/create', '/families/create', Record<never, never>, Record<never, never>>,
    '/family': RouteRecordInfo<'/family', '/family', Record<never, never>, Record<never, never>>,
    '/home': RouteRecordInfo<'/home', '/home', Record<never, never>, Record<never, never>>,
    '/posts/[id]': RouteRecordInfo<'/posts/[id]', '/posts/:id', { id: ParamValue<true> }, { id: ParamValue<false> }>,
    '/posts/create': RouteRecordInfo<'/posts/create', '/posts/create', Record<never, never>, Record<never, never>>,
    '/posts/update/[id]': RouteRecordInfo<'/posts/update/[id]', '/posts/update/:id', { id: ParamValue<true> }, { id: ParamValue<false> }>,
    '/sign-up': RouteRecordInfo<'/sign-up', '/sign-up', Record<never, never>, Record<never, never>>,
    '/users/create': RouteRecordInfo<'/users/create', '/users/create', Record<never, never>, Record<never, never>>,
  }
}
