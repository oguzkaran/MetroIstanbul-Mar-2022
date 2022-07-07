#ifndef UTIL_H_
#define UTIL_H_

#ifdef __cplusplus
extern "C" {
#endif
void exit_sys(const char *msg);
void exit_sys_thread(const char *msg, int errnum);
#ifdef __cplusplus
}
#endif
#endif /* UTIL_H_ */
